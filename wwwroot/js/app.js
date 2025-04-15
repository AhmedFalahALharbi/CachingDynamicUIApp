// DOM Elements
const loadUsersBtn = document.getElementById('loadUsers');
const loadPostsBtn = document.getElementById('loadPosts');
const usersContainer = document.getElementById('users-container');
const postsContainer = document.getElementById('posts-container');
const userDetailContainer = document.getElementById('user-detail');
const usersGrid = document.getElementById('users');
const postsGrid = document.getElementById('posts');
const userInfo = document.getElementById('user-info');
const userPosts = document.getElementById('user-posts');
const backButton = document.getElementById('backButton');
const loader = document.getElementById('loader');

// API URLs
const API_BASE_URL = '/api';
const USERS_URL = `${API_BASE_URL}/users`;
const POSTS_URL = `${API_BASE_URL}/posts`;

// Event Listeners
loadUsersBtn.addEventListener('click', loadUsers);
loadPostsBtn.addEventListener('click', loadPosts);
backButton.addEventListener('click', showUsersList);

// Load users from API
async function loadUsers() {
    showLoader();
    hideContainers();
    
    try {
        const response = await fetch(USERS_URL);
        if (!response.ok) {
            throw new Error('Failed to fetch users');
        }
        
        const users = await response.json();
        renderUsers(users);
        
        usersContainer.classList.remove('hidden');
    } catch (error) {
        console.error('Error loading users:', error);
        alert('Error loading users. Please try again later.');
    } finally {
        hideLoader();
    }
}

// Load posts from API
async function loadPosts() {
    showLoader();
    hideContainers();
    
    try {
        const response = await fetch(POSTS_URL);
        if (!response.ok) {
            throw new Error('Failed to fetch posts');
        }
        
        const posts = await response.json();
        renderPosts(posts);
        
        postsContainer.classList.remove('hidden');
    } catch (error) {
        console.error('Error loading posts:', error);
        alert('Error loading posts. Please try again later.');
    } finally {
        hideLoader();
    }
}

// Load user details
async function loadUserDetails(userId) {
    showLoader();
    hideContainers();
    
    try {
        // Fetch user details
        const userResponse = await fetch(`${USERS_URL}/${userId}`);
        if (!userResponse.ok) {
            throw new Error('Failed to fetch user details');
        }
        const user = await userResponse.json();
        
        // Fetch user posts
        const postsResponse = await fetch(`${USERS_URL}/${userId}/posts`);
        if (!postsResponse.ok) {
            throw new Error('Failed to fetch user posts');
        }
        const posts = await postsResponse.json();
        
        renderUserDetails(user, posts);
        
        userDetailContainer.classList.remove('hidden');
    } catch (error) {
        console.error('Error loading user details:', error);
        alert('Error loading user details. Please try again later.');
    } finally {
        hideLoader();
    }
}

// Render users
function renderUsers(users) {
    usersGrid.innerHTML = '';
    
    users.forEach(user => {
        const userCard = document.createElement('div');
        userCard.className = 'card';
        userCard.dataset.userId = user.id;
        
        const avatarUrl = `/api/images/avatar/${encodeURIComponent(user.email)}`;
                
        userCard.innerHTML = `
            <img class="user-avatar" src="${avatarUrl}" alt="${user.name}" loading="lazy">
            <h3>${user.name}</h3>
            <p><strong>Email:</strong> ${user.email}</p>
            <p><strong>Company:</strong> ${user.company.name}</p>
        `;
        
        userCard.addEventListener('click', () => loadUserDetails(user.id));
        
        usersGrid.appendChild(userCard);
    });
}

// Render posts
function renderPosts(posts) {
    postsGrid.innerHTML = '';
    
    posts.forEach(post => {
        const postCard = document.createElement('div');
        postCard.className = 'card';
        
        postCard.innerHTML = `
            <h3>${post.title}</h3>
            <p>${post.body}</p>
            <p><strong>User ID:</strong> ${post.userId}</p>
        `;
        
        postsGrid.appendChild(postCard);
    });
}

// Render user details
function renderUserDetails(user, posts) {
    // User info
    const avatarUrl = `https://i.pravatar.cc/150?u=${user.email}`;
    
    userInfo.innerHTML = `
        <div style="display: flex; align-items: center; margin-bottom: 1rem;">
            <img class="user-avatar" src="${avatarUrl}" alt="${user.name}" style="margin-right: 1rem;">
            <div>
                <h2>${user.name}</h2>
                <p><strong>Username:</strong> ${user.username}</p>
                <p><strong>Email:</strong> ${user.email}</p>
                <p><strong>Phone:</strong> ${user.phone}</p>
                <p><strong>Website:</strong> ${user.website}</p>
            </div>
        </div>
        <div>
            <p><strong>Address:</strong> ${user.address.street}, ${user.address.suite}, ${user.address.city}, ${user.address.zipcode}</p>
            <p><strong>Company:</strong> ${user.company.name} - "${user.company.catchPhrase}"</p>
        </div>
    `;
    
    // User posts
    userPosts.innerHTML = '';
    
    if (posts.length === 0) {
        userPosts.innerHTML = '<p>No posts found for this user.</p>';
    } else {
        posts.forEach(post => {
            const postCard = document.createElement('div');
            postCard.className = 'card';
            
            postCard.innerHTML = `
                <h3>${post.title}</h3>
                <p>${post.body}</p>
            `;
            
            userPosts.appendChild(postCard);
        });
    }
}

// Show loader
function showLoader() {
    loader.classList.remove('hidden');
}

// Hide loader
function hideLoader() {
    loader.classList.add('hidden');
}

// Hide all containers
function hideContainers() {
    usersContainer.classList.add('hidden');
    postsContainer.classList.add('hidden');
    userDetailContainer.classList.add('hidden');
}

// Show users list
function showUsersList() {
    hideContainers();
    usersContainer.classList.remove('hidden');
}

// Initialize app
function init() {
    // Load users by default
    loadUsers();
}

// Start the app
init();