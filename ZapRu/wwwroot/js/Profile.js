let currentUser = null;
let userCars = [];
let currentCarIndex = 0;
let partsCatalog = [];
let carModels = [];

function showMessage(containerId, text, isError = true) {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = `<div class="${isError ? 'error-msg' : 'success-msg'}">${text}</div>`;
    setTimeout(() => { if (container.innerHTML.includes(text)) container.innerHTML = ''; }, 5000);
}

async function loadCatalogs() {
    const [modelsRes, partsRes] = await Promise.all([
        fetch('/api/carmodels', { credentials: 'include' }),
        fetch('/api/partscatalog', { credentials: 'include' })
    ]);
    if (modelsRes.ok) carModels = await modelsRes.json();
    if (partsRes.ok) partsCatalog = await partsRes.json();
}

async function loadUserProfile() {
    const res = await fetch('/api/profile/me', { credentials: 'include' });
    if (res.ok) {
        currentUser = await res.json();
        const userNameEl = document.getElementById('userName');
        if (userNameEl) userNameEl.innerText = `${currentUser.last_name} ${currentUser.first_name}`;
        const userEmailEl = document.getElementById('userEmail');
        if (userEmailEl) userEmailEl.innerText = currentUser.email;
        const userPhoneEl = document.getElementById('userPhone');
        if (userPhoneEl) userPhoneEl.innerText = currentUser.phone;
    }
}

async function loadMyAds() {
    const res = await fetch('/api/profile/advertisements', { credentials: 'include' });
    if (!res.ok) return;
    userCars = await res.json();
    currentCarIndex = 0;
    renderCarousel();
}

function renderCarousel() {
    const container = document.getElementById('adsCarouselInner');
    if (!userCars.length) {
        container.innerHTML = '<p>У вас пока нет объявлений. Создайте первое!</p>';
        return;
    }
    const car = userCars[currentCarIndex];
    const parts = car.parts || [];
    const photos = car.photos || [];
    let html = `
        <div class="ad-card">
            <div class="ad-actions">
                <button class="edit-ad-btn" data-id="${car.carID}">Редактировать</button>
                <button class="delete-ad-btn" data-id="${car.carID}">Удалить</button>
            </div>
            <h3>${car.brand} ${car.model} (${car.manufactureYear})</h3>
            <div>Пробег: ${car.mileage || '?'} км • Регион: ${car.region}</div>
            <div class="stats">Просмотров всех деталей: ${parts.reduce((s, p) => s + (p.views_Count || 0), 0)}</div>
            <div class="car-photos-preview">
                ${photos.map(p => `<img src="${p}" style="width:80px; height:80px; object-fit:cover;">`).join('')}
            </div>
            <h4>Детали:</h4>
            <ul>
                ${parts.map(p => `<li>${p.name || ''} — ${p.price || ''}₽ (откликов: ${p.bidsCount || 0})</li>`).join('')}
            </ul>
        </div>
    `;
    container.innerHTML = html;

    document.querySelectorAll('.edit-ad-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            const id = parseInt(btn.dataset.id);
            editCar(id);
        });
    });
    document.querySelectorAll('.delete-ad-btn').forEach(btn => {
        btn.addEventListener('click', async () => {
            if (confirm('Удалить объявление? Все фотографии и детали будут удалены безвозвратно.')) {
                const id = btn.dataset.id;
                await fetch(`/api/profile/car/${id}`, { method: 'DELETE', credentials: 'include' });
                loadMyAds();
            }
        });
    });

    const prevBtn = document.getElementById('prevAdBtn');
    const nextBtn = document.getElementById('nextAdBtn');
    if (prevBtn) {
        const newPrevBtn = prevBtn.cloneNode(true);
        prevBtn.parentNode.replaceChild(newPrevBtn, prevBtn);
        newPrevBtn.addEventListener('click', () => prevAd());
    }
    if (nextBtn) {
        const newNextBtn = nextBtn.cloneNode(true);
        nextBtn.parentNode.replaceChild(newNextBtn, nextBtn);
        newNextBtn.addEventListener('click', () => nextAd());
    }
}

function prevAd() {
    if (userCars.length === 0) return;
    currentCarIndex = (currentCarIndex - 1 + userCars.length) % userCars.length;
    renderCarousel();
}
function nextAd() {
    if (userCars.length === 0) return;
    currentCarIndex = (currentCarIndex + 1) % userCars.length;
    renderCarousel();
}

function editCar(carId) {
    const car = userCars.find(c => c.carID === carId);
    if (!car) return;
    document.getElementById('editCarId').value = car.carID;
    document.getElementById('carModelId').value = car.carModelID;
    document.getElementById('year').value = car.manufactureYear;
    document.getElementById('mileage').value = car.mileage || '';
    document.getElementById('condition').value = car.condition || '';
    document.getElementById('region').value = car.region || '';
    document.getElementById('description').value = car.description || '';
    const partsContainer = document.getElementById('partsContainer');
    partsContainer.innerHTML = '';

    (car.parts || []).forEach(part => {
        addPartRow(part.partCatID, part.price, part.condition);
    });
    document.querySelector('.tab-btn[data-tab="create"]').click();
}

function addPartRow(selectedCatId = null, price = '', condition = 'Б/у') {
    const container = document.getElementById('partsContainer');
    const row = document.createElement('div');
    row.className = 'part-row';
    const select = document.createElement('select');
    select.innerHTML = '<option value="">Выберите деталь</option>';
    partsCatalog.forEach(cat => {
        const opt = document.createElement('option');
        opt.value = cat.partCatID;
        opt.text = `${cat.name} (${cat.category})`;
        if (selectedCatId && selectedCatId == cat.partCatID) opt.selected = true;
        select.appendChild(opt);
    });
    const priceInput = document.createElement('input');
    priceInput.type = 'number';
    priceInput.placeholder = 'Цена';
    priceInput.value = price || '';
    const condInput = document.createElement('input');
    condInput.placeholder = 'Состояние';
    condInput.value = condition || '';
    const delBtn = document.createElement('button');
    delBtn.type = 'button';
    delBtn.textContent = '✖';
    delBtn.onclick = () => row.remove();
    row.append(select, priceInput, condInput, delBtn);
    container.appendChild(row);
}

async function uploadCarPhotos(carId, files) {
    if (!files.length) return;
    const formData = new FormData();
    for (let file of files) formData.append('files', file);
    const res = await fetch(`/api/profile/car/${carId}/photos`, {
        method: 'POST',
        body: formData,
        credentials: 'include'
    });
    if (!res.ok) console.error('Ошибка загрузки фото');
}

document.getElementById('adForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const carId = document.getElementById('editCarId').value;
    const carData = {
        carModelID: parseInt(document.getElementById('carModelId').value),
        year: parseInt(document.getElementById('year').value),
        mileage: document.getElementById('mileage').value ? parseInt(document.getElementById('mileage').value) : null,
        condition: document.getElementById('condition').value,
        region: document.getElementById('region').value,
        description: document.getElementById('description').value,
        parts: []
    };
    const partRows = document.querySelectorAll('.part-row');
    for (let row of partRows) {
        const select = row.querySelector('select');
        const priceInput = row.querySelector('input[type="number"]');
        const condInput = row.querySelectorAll('input')[1];
        if (select.value) {
            carData.parts.push({
                partCatID: parseInt(select.value),
                price: priceInput.value ? parseFloat(priceInput.value) : null,
                condition: condInput.value
            });
        }
    }
    const url = carId ? `/api/profile/car/${carId}` : '/api/profile/car';
    const method = carId ? 'PUT' : 'POST';
    const res = await fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(carData),
        credentials: 'include'
    });
    if (res.ok) {
        const result = await res.json();

        const newCarId = result.carID || carId;
        const photoFiles = document.getElementById('carPhotos').files;
        if (photoFiles.length && newCarId) {
            await uploadCarPhotos(newCarId, Array.from(photoFiles));
        }
        loadMyAds();
        document.getElementById('editCarId').value = '';
        document.getElementById('adForm').reset();
        document.getElementById('partsContainer').innerHTML = '';
        addPartRow();
        alert('Объявление сохранено');
        document.querySelector('.tab-btn[data-tab="ads"]').click();
    } else {
        const err = await res.json();
        showMessage('createMessage', err.message || 'Ошибка сохранения', true);
    }
});

document.getElementById('addPartBtn').addEventListener('click', () => {
    addPartRow();
});

async function loadBids() {
    const res = await fetch('/api/profile/bids/received', { credentials: 'include' });
    if (!res.ok) return;
    const bids = await res.json();
    const container = document.getElementById('bidsList');
    if (!bids.length) {
        container.innerHTML = '<p>Нет откликов</p>';
        return;
    }
  
    container.innerHTML = bids.map(b => `
        <div class="bid-card" data-id="${b.bidID}">
            <div>Деталь: ${b.partName} (${b.carName})</div>
            <div>Покупатель: ${b.buyerName}</div>
            <div>Контакты: ${b.buyerPhone}, ${b.buyerEmail}</div>
            <div>Предложено: ${b.offeredPrice}₽</div>
            <div>Сообщение: ${b.comment || ''}</div>
            <button class="accept-bid" data-id="${b.bidID}"> Принять</button>
            <button class="reject-bid" data-id="${b.bidID}"> Отклонить</button>
        </div>
    `).join('');

    document.querySelectorAll('.accept-bid').forEach(btn => {
        btn.addEventListener('click', async () => {
            const id = btn.dataset.id;
            await fetch(`/api/profile/bid/${id}/accept`, { method: 'POST', credentials: 'include' });
            loadBids();
            loadMyAds();
        });
    });
    document.querySelectorAll('.reject-bid').forEach(btn => {
        btn.addEventListener('click', async () => {
            const id = btn.dataset.id;
            await fetch(`/api/profile/bid/${id}/reject`, { method: 'POST', credentials: 'include' });
            loadBids();
        });
    });
}

async function loadPurchases() {
    const res = await fetch('/api/profile/purchases', { credentials: 'include' });
    if (!res.ok) return;
    const purchases = await res.json();
    const container = document.getElementById('purchasesList');
    if (!purchases.length) {
        container.innerHTML = '<p>Нет покупок</p>';
        return;
    }

    container.innerHTML = purchases.map(p => `
        <div class="purchase-card">
            <div>Товар: ${p.partName} (${p.carName})</div>
            <div>Продавец: ${p.sellerName}</div>
            <div>Телефон: ${p.sellerPhone}</div>
            <div>Email: ${p.sellerEmail}</div>
            <div>Сумма: ${p.agreedPrice}₽</div>
            <div>Статус: ${p.status}</div>
        </div>
    `).join('');
}

document.getElementById('editProfileBtn').addEventListener('click', async () => {
    const newFirstName = prompt('Имя', currentUser.first_name);
    if (!newFirstName) return;
    const newLastName = prompt('Фамилия', currentUser.last_name);
    const newMiddle = prompt('Отчество', currentUser.middle_name || '');
    const newEmail = prompt('Email', currentUser.email);
    const newPhone = prompt('Телефон', currentUser.phone);
    const res = await fetch('/api/profile/me', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            firstName: newFirstName,
            lastName: newLastName,
            middleName: newMiddle,
            email: newEmail,
            phone: newPhone
        }),
        credentials: 'include'
    });
    if (res.ok) {
        location.reload();
    } else {
        alert('Ошибка обновления профиля');
    }
});

function initTabs() {
    const tabs = document.querySelectorAll('.tab-btn');
    const contents = document.querySelectorAll('.tab-content');
    tabs.forEach(btn => {
        btn.addEventListener('click', () => {
            const tabId = btn.getAttribute('data-tab');
            tabs.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            contents.forEach(c => c.classList.remove('active'));
            document.getElementById(`${tabId}Tab`).classList.add('active');
            if (tabId === 'ads') loadMyAds();
            if (tabId === 'bids') loadBids();
            if (tabId === 'purchases') loadPurchases();
            if (tabId === 'create' && document.getElementById('partsContainer').children.length === 0) {
                addPartRow();
            }
        });
    });
}

document.addEventListener('DOMContentLoaded', async () => {
    await loadCatalogs();
    await loadUserProfile();
    initTabs();
    const modelSelect = document.getElementById('carModelId');
    carModels.forEach(m => {
        const opt = document.createElement('option');
        opt.value = m.carModelID;
        opt.text = `${m.brand} ${m.model}`;
        modelSelect.appendChild(opt);
    });
    loadMyAds();
    addPartRow();
});