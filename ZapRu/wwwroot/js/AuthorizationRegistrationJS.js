
const API_BASE = window.location.origin + '/api/auth';

function isValidPhone(phone) {
    const digits = phone.replace(/\D/g, '');

    if (digits.length === 11 && (digits[0] === '7' || digits[0] === '8')) {
        return true;
    }
    return false;
}

function showMessage(containerId, text, isError = true) {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = `<div class="${isError ? 'error-msg' : 'success-msg'}"><i class="fas ${isError ? 'fa-exclamation-circle' : 'fa-check-circle'}"></i> ${text}</div>`;
    setTimeout(() => {
        if (container.innerHTML.includes(text)) container.innerHTML = '';
    }, 5000);
}


function isValidEmail(email) {
    const re = /^[^\s@]+@([^\s@]+\.)+[^\s@]+$/;
    return re.test(email);
}


function setAuthState(userData) {
    sessionStorage.setItem('isLoggedIn', 'true');
    if (userData) {
        sessionStorage.setItem('user', JSON.stringify(userData));
    }
}


function clearAuthState() {
    sessionStorage.removeItem('isLoggedIn');
    sessionStorage.removeItem('user');
    localStorage.removeItem('user'); 
}


window.logout = async function (redirectUrl = '/login.html') {
    try {
        const response = await fetch(`${API_BASE}/logout`, {
            method: 'POST',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' }
        });
        clearAuthState();
        if (response.ok) {
            window.location.href = "/Home/Market";
        } else {
            console.error('Ошибка при выходе:', response.status);
            alert('Не удалось выйти. Попробуйте позже.');
        }
    } catch (error) {
        console.error('Сетевая ошибка:', error);
        alert('Сервер недоступен. Попробуйте позже.');
    }
};


const tabBtns = document.querySelectorAll('.tab-btn');
const loginPanel = document.getElementById('login-panel');
const registerPanel = document.getElementById('register-panel');
const switchToReg = document.getElementById('switch-to-reg');
const switchToLogin = document.getElementById('switch-to-login');

function setActiveTab(tabId) {
    tabBtns.forEach(btn => {
        btn.classList.remove('active');
        if (btn.getAttribute('data-tab') === tabId) btn.classList.add('active');
    });
    if (tabId === 'login') {
        loginPanel.classList.add('active');
        registerPanel.classList.remove('active');
    } else {
        loginPanel.classList.remove('active');
        registerPanel.classList.add('active');
    }
}

tabBtns.forEach(btn => {
    btn.addEventListener('click', () => setActiveTab(btn.getAttribute('data-tab')));
});
switchToReg?.addEventListener('click', (e) => { e.preventDefault(); setActiveTab('register'); });
switchToLogin?.addEventListener('click', (e) => { e.preventDefault(); setActiveTab('login'); });


const loginForm = document.getElementById('login-form');
loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const email = document.getElementById('login-email').value.trim();
    const password = document.getElementById('login-password').value;
    const rememberMe = document.getElementById('remember').checked;

    
    if (!email || !password) {
        showMessage('login-message', 'Заполните все поля', true);
        return;
    }
    if (!isValidEmail(email)) {
        showMessage('login-message', 'Введите корректный адрес электронной почты', true);
        return;
    }

    const submitBtn = loginForm.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;
    submitBtn.disabled = true;
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-pulse"></i> Вход...';

    try {
        const response = await fetch(`${API_BASE}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password, rememberMe }),
            credentials: 'include'
        });
        const data = await response.json();
        if (response.ok) {
            
            setAuthState({ email, name: data.userName || '' });
            showMessage('login-message', 'Успешный вход! Перенаправление...', false);
            setTimeout(() => {
                window.location.href = "/Home/Market";
            }, 1200);
        } else {
            showMessage('login-message', data.message || 'Неверный email или пароль', true);
            submitBtn.disabled = false;
            submitBtn.innerHTML = originalText;
        }
    } catch (error) {
        console.error('Ошибка соединения:', error);
        showMessage('login-message', 'Сервер временно недоступен. Попробуйте позже.', true);
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalText;
    }
});


const registerForm = document.getElementById('register-form');
registerForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const lastName = document.getElementById('reg-lastname').value.trim();
    const firstName = document.getElementById('reg-firstname').value.trim();
    const middleName = document.getElementById('reg-middlename').value.trim();
    const email = document.getElementById('reg-email').value.trim();
    const phone = document.getElementById('reg-phone').value.trim();
    const password = document.getElementById('reg-password').value;
    const confirm = document.getElementById('reg-password-confirm').value;
    const agree = document.getElementById('agree-terms').checked;

   
    if (!lastName || !firstName || !email || !phone || !password) {
        showMessage('register-message', 'Пожалуйста, заполните обязательные поля', true);
        return;
    }

    if (!isValidPhone(phone)) {
        showMessage('register-message', 'Введите корректный номер телефона', true);
        return;
    }

    if (!isValidEmail(email)) {
        showMessage('register-message', 'Введите корректный адрес электронной почты', true);
        return;
    }
    if (password !== confirm) {
        showMessage('register-message', 'Пароли не совпадают', true);
        return;
    }
    if (password.length < 8) {
        showMessage('register-message', 'Пароль должен содержать минимум 8 символов', true);
        return;
    }
    if (!agree) {
        showMessage('register-message', 'Необходимо принять условия соглашения', true);
        return;
    }

    const submitBtn = registerForm.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;
    submitBtn.disabled = true;
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-pulse"></i> Регистрация...';

    try {
        const response = await fetch(`${API_BASE}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ lastName, firstName, middleName, email, phone, password }),
            credentials: 'include'
        });
        const data = await response.json();
        if (response.ok) {
            showMessage('register-message', 'Регистрация успешна! Теперь войдите.', false);
            
            setActiveTab('login');
            document.getElementById('login-email').value = email;
            document.getElementById('login-password').value = '';
            document.getElementById('remember').checked = false;
           
            setTimeout(() => {
                const msgDiv = document.getElementById('register-message');
                if (msgDiv) msgDiv.innerHTML = '';
            }, 3000);
        } else {
            showMessage('register-message', data.message || 'Ошибка регистрации (возможно, email уже используется)', true);
        }
    } catch (error) {
        console.error('Ошибка сети:', error);
        showMessage('register-message', 'Не удалось соединиться с сервером', true);
    } finally {
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalText;
    }
});


const clearOnInput = (ids, containerId) => {
    ids.forEach(id => {
        const el = document.getElementById(id);
        if (el) el.addEventListener('input', () => {
            const container = document.getElementById(containerId);
            if (container) container.innerHTML = '';
        });
    });
};
clearOnInput(['login-email', 'login-password'], 'login-message');
clearOnInput(['reg-email', 'reg-password', 'reg-password-confirm', 'reg-firstname', 'reg-lastname'], 'register-message');