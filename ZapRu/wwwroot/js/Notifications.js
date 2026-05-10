const API_BASE = window.location.origin + '/api';

async function loadUserInfo() {
    try {
        const response = await fetch('/api/auth/me', { credentials: 'include' });
        if (response.ok) {
            const user = await response.json();
            const fullName = `${user.lastName} ${user.firstName}`;
            const userNameSpan = document.getElementById('userFullName');
            if (userNameSpan) userNameSpan.innerText = fullName;
        } else {
            console.log('Пользователь не авторизован');
        }
    } catch (error) {
        console.error('Ошибка загрузки профиля', error);
    }
}

function loadNotifications() {
    fetch(API_BASE + '/notifications', { credentials: 'include' })
        .then(res => res.ok ? res.json() : [])
        .then(notifications => {
            const unreadCount = notifications.filter(n => !n.isRead).length;
            const badge = document.getElementById('notificationBadge');
            if (badge) {
                if (unreadCount > 0) {
                    badge.style.display = 'inline-block';
                    badge.innerText = unreadCount > 99 ? '99+' : unreadCount;
                } else {
                    badge.style.display = 'none';
                }
            }
            renderNotificationList(notifications);
        })
        .catch(console.error);
}

function renderNotificationList(notifications) {
    const container = document.getElementById('notificationList');
    if (!container) return;
    if (!notifications.length) {
        container.innerHTML = '<div class="notification-item">Нет уведомлений</div>';
        return;
    }
    container.innerHTML = notifications.map(n => `
        <div class="notification-item ${n.isRead ? '' : 'unread'}" data-id="${n.notificationId}">
            <div>${n.message}</div>
            <small>${new Date(n.createdAt).toLocaleString()}</small>
            ${n.type === 'bid' && n.ResponseStatus === null ? `
                <div class="bid-actions">
                    <button class="btn-accept" data-id="${n.notificationId}">Принять</button>
                    <button class="btn-decline" data-id="${n.notificationId}">Отклонить</button>
                </div>
            ` : (n.responseStatus !== null ? `<div>${n.responseStatus ? 'Принято' : 'Отклонено'}</div>` : '')}
        </div>
    `).join('');

    document.querySelectorAll('.btn-accept').forEach(btn => {
        btn.addEventListener('click', async (e) => {
            e.stopPropagation();
            const id = btn.dataset.id;
            await fetch(API_BASE + `/notifications/${id}/respond`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ accept: true, price: null }),
                credentials: 'include'
            });
            loadNotifications();
        });
    });
    document.querySelectorAll('.btn-decline').forEach(btn => {
        btn.addEventListener('click', async (e) => {
            e.stopPropagation();
            const id = btn.dataset.id;
            await fetch(API_BASE + `/notifications/${id}/respond`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ accept: false, price: null }),
                credentials: 'include'
            });
            loadNotifications();
        });
    });
}

function initDropdowns() {
    const notifIcon = document.getElementById('notificationIcon');
    const notifDropdown = document.getElementById('notificationDropdown');
    const profileIcon = document.getElementById('profileIcon');
    const profileDropdown = document.getElementById('profileDropdown');

    if (notifIcon && notifDropdown) {
        notifIcon.addEventListener('click', (e) => {
            e.stopPropagation();
            notifDropdown.style.display = notifDropdown.style.display === 'block' ? 'none' : 'block';
            if (profileDropdown) profileDropdown.style.display = 'none';
            loadNotifications();
        });
    }
    if (profileIcon && profileDropdown) {
        profileIcon.addEventListener('click', (e) => {
            e.stopPropagation();
            profileDropdown.style.display = profileDropdown.style.display === 'block' ? 'none' : 'block';
            if (notifDropdown) notifDropdown.style.display = 'none';
        });
    }
    document.addEventListener('click', () => {
        if (notifDropdown) notifDropdown.style.display = 'none';
        if (profileDropdown) profileDropdown.style.display = 'none';
    });
}

document.getElementById('logoutBtn')?.addEventListener('click', async (e) => {
    e.preventDefault();
    await fetch(API_BASE + '/auth/logout', { method: 'POST', credentials: 'include' });
    window.location.href = '/Home/Authorization';
});

document.addEventListener('DOMContentLoaded', () => {
    loadUserInfo();
    initDropdowns();
});