async function loadMarket() {
    try {
        const res = await fetch('/api/market/users', { credentials: 'include' });
        if (!res.ok) throw new Error('Ошибка загрузки');
        const data = await res.json();
        console.log('Ответ API:', data);
        const users = Array.isArray(data) ? data : [];
        const grid = document.getElementById('usersGrid');
        grid.innerHTML = '';
        if (!users.length) {
            grid.innerHTML = '<div style="text-align:center;">Нет объявлений</div>';
            return;
        }
        users.forEach(user => {
            const cars = user.cars;
            if (!cars || !cars.length) return;
            cars.forEach(car => {
                const card = document.createElement('div');
                card.className = 'user-card';

                const carouselId = `car-${car.carID}`;
                const firstName = user.firstName || '';
                const lastName = user.lastName || '';
                const phone = user.phone || '';
                const brand = car.brand || '';
                const model = car.model || '';
                const manufactureYear = car.manufactureYear || '';
                const photos = car.photos || [];
                const avatarInitials = (firstName[0] || '') + (lastName[0] || '');
                const parts = car.parts || [];
                card.innerHTML = `
                    <div class="user-info">
                        <div class="avatar">${avatarInitials}</div>
                        <div class="user-details">
                            <h3>${lastName} ${firstName}</h3>
                            <div class="user-meta">
                                <span><i class="fas fa-phone-alt"></i> ${phone}</span>
                                <span><i class="fas fa-car"></i> ${brand} ${model} (${manufactureYear})</span>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-container" data-carousel="${carouselId}">
                        <div class="carousel-track">
                            <div class="carousel-slides" id="slides-${carouselId}" style="display: flex;"></div>
                        </div>
                        <button class="carousel-btn btn-prev" data-carousel="${carouselId}"><i class="fas fa-chevron-left"></i></button>
                        <button class="carousel-btn btn-next" data-carousel="${carouselId}"><i class="fas fa-chevron-right"></i></button>
                        <div class="carousel-dots" id="dots-${carouselId}"></div>
                    </div>
                    <div class="parts-list">
                        <h4>Запчасти:</h4>
                        ${parts.length ? parts.filter(p => !p.isSold).map(p => `
                            <div class="part-item">
                                <span class="part-name">${p.name}</span>
                                <span class="part-price">${p.price ? p.price + '₽' : 'Цена не указана'}</span>
                                <span class="part-condition">${p.condition || ''}</span>
                                <button class="btn btn-secondary bid-btn" 
                                    data-part-id="${p.partID}" 
                                    data-part-name="${p.name}">
                                    Откликнуться
                                </button>
                            </div>
                        `).join('') : '<p>Нет доступных деталей</p>'}
                    </div>
                `;
                grid.appendChild(card);

                card.querySelectorAll('.bid-btn').forEach(btn => {
                    btn.addEventListener('click', () => {
                        const partId = btn.dataset.partId;
                        const partName = btn.dataset.partName;
                        const price = prompt(`Ваше предложение за "${partName}" (₽):`);
                        if (!price) return;
                        const comment = prompt('Комментарий (необязательно):') || '';
                        fetch(`/api/profile/${partId}/bid`, {
                            method: 'POST',
                            headers: { 'Content-Type': 'application/json' },
                            body: JSON.stringify({ offeredPrice: parseFloat(price), comment }),
                            credentials: 'include'
                        }).then(res => {
                            if (res.ok) alert('Отклик отправлен!');
                            else alert('Ошибка при отправке отклика');
                        });
                    });
                });
                initCarouselForCar(card, photos, carouselId);
            });
        });
    } catch (error) {
        console.error(error);
        document.getElementById('usersGrid').innerHTML = '<div style="text-align:center; color:red;">Не удалось загрузить стенды</div>';
    }
}

function initCarouselForCar(card, photos, carouselId) {
    const slidesContainer = card.querySelector(`#slides-${carouselId}`);
    const prevBtn = card.querySelector(`.btn-prev[data-carousel="${carouselId}"]`);
    const nextBtn = card.querySelector(`.btn-next[data-carousel="${carouselId}"]`);
    const dotsContainer = card.querySelector(`#dots-${carouselId}`);
    if (!dotsContainer || !slidesContainer) return;
    let currentIndex = 0;
    const total = photos.length;
    function update() {
        slidesContainer.style.transform = `translateX(-${currentIndex * 100}%)`;
        const dots = dotsContainer.querySelectorAll('.dot');
        dots.forEach((dot, idx) => dot.classList.toggle('active', idx === currentIndex));
    }
    photos.forEach((photo, idx) => {
        const slide = document.createElement('div');
        slide.className = 'carousel-slide';
        slide.innerHTML = `<img src="${photo}" alt="Фото" loading="lazy">`;
        slidesContainer.appendChild(slide);
        const dot = document.createElement('div');
        dot.className = `dot ${idx === 0 ? 'active' : ''}`;
        dot.addEventListener('click', () => { currentIndex = idx; update(); });
        dotsContainer.appendChild(dot);
    });
    if (prevBtn) prevBtn.addEventListener('click', () => { currentIndex = (currentIndex - 1 + total) % total; update(); });
    if (nextBtn) nextBtn.addEventListener('click', () => { currentIndex = (currentIndex + 1) % total; update(); });
    if (total === 0) {
        slidesContainer.innerHTML = '<div class="carousel-slide">Нет фото</div>';
        if (prevBtn) prevBtn.style.display = 'none';
        if (nextBtn) nextBtn.style.display = 'none';
    }
}

document.addEventListener("DOMContentLoaded", function () {
    loadMarket();
});