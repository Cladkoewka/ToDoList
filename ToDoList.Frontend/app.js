// Handling Tag creation
document.getElementById('addTagForm').addEventListener('submit', async function(event) {
    event.preventDefault();
    const tagName = document.getElementById('tagName').value;
    
    const response = await fetch('http://localhost:5153/api/tags', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name: tagName })
    });

    if (response.ok) {
        alert('Tag added successfully');
        document.getElementById('tagName').value = '';
        loadTags(); // Обновить список тегов
    } else {
        alert('Error adding tag');
    }
});

// Update the link to the element for displaying tags
async function loadTags() {
    try {
        const response = await fetch('http://localhost:5153/api/tags');
        
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        
        const tags = await response.json();
        console.log('Tags fetched:', tags); // Для отладки

        const tagList = document.getElementById('tagList');
        tagList.innerHTML = '';

        if (tags.length === 0) {
            console.log('No tags available');
        }

        tags.forEach(tag => {
            const listItem = document.createElement('li');
            listItem.textContent = tag.name;

            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Delete';
            deleteButton.className = 'delete-tag-btn';
            deleteButton.dataset.id = tag.id; // Установить ID тега как дата-атрибут

            listItem.appendChild(deleteButton);
            tagList.appendChild(listItem);
        });

        // Привязка обработчика к кнопкам удаления тегов
        document.querySelectorAll('.delete-tag-btn').forEach(button => {
            button.addEventListener('click', handleDeleteTag);
        });
    } catch (error) {
        console.error('Error fetching tags:', error);
    }
}

// Handle tag deletion
async function handleDeleteTag(event) {
    const tagId = event.target.dataset.id;
    const response = await fetch(`http://localhost:5153/api/tags/${tagId}`, {
        method: 'DELETE'
    });

    if (response.ok) {
        alert('Tag deleted successfully');
        loadTags(); // Перезагрузить список тегов
    } else {
        alert('Error deleting tag');
    }
}


// Привязка обработчика к кнопке
document.getElementById('getAllTags').addEventListener('click', loadTags);


// Handling Task creation
document.getElementById('addTaskForm').addEventListener('submit', async function(event) {
    event.preventDefault();
    const taskTitle = document.getElementById('taskTitle').value;
    const taskDescription = document.getElementById('taskDescription').value;
    const taskTags = Array.from(document.getElementById('taskTags').selectedOptions).map(option => option.value);
    
    const currentUserId = await getCurrentUser(); // Получить текущего пользователя

    const createdTime = new Date().toISOString();  // Автоматически устанавливается текущее время
    const lastUpdateTime = new Date().toISOString();  // Автоматически устанавливается текущее время

    try {
        const response = await fetch('http://localhost:5153/api/tasks', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                title: taskTitle,
                description: taskDescription,
                createdTime: createdTime,
                lastUpdateTime: lastUpdateTime,
                userId: currentUserId, 
                tagIds: taskTags.map(Number) // Преобразуем ID тегов в числа
            })
        });

        if (!response.ok) {
            const errorText = await response.text(); // Получить текст ошибки
            throw new Error(`Error ${response.status}: ${errorText}`);
        }

        alert('Task added successfully');
        document.getElementById('addTaskForm').reset();
    } catch (error) {
        console.error('Error adding task:', error);
        alert('Error adding task: ' + error.message); // Показать более подробное сообщение об ошибке
    }
});




// Convert date to dd:mm:yyyy HH:mm format
function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${day}:${month}:${year} ${hours}:${minutes}`;
}

// Fetch and display all tasks
async function loadTasks() {
    try {
        // Загрузка задач и пользователей параллельно
        const [tasksResponse, usersResponse] = await Promise.all([
            fetch('http://localhost:5153/api/tasks'),
            fetch('http://localhost:5153/api/users')
        ]);

        if (!tasksResponse.ok || !usersResponse.ok) {
            throw new Error('Network response was not ok');
        }

        const tasks = await tasksResponse.json();
        const users = await usersResponse.json();

        // Создаем маппинг ID пользователей на имена
        const userMap = new Map(users.map(user => [user.id, user.username]));

        const taskList = document.getElementById('taskList');
        taskList.innerHTML = '';

        tasks.forEach(task => {
            // Форматирование дат
            const createdTime = formatDate(task.createdTime);
            const lastUpdateTime = formatDate(task.lastUpdateTime);
            
            // Форматирование тегов
            const tags = task.tags.length > 0 ? task.tags.map(tag => tag.name).join(', ') : 'No tags';

            // Получаем имя пользователя из маппинга
            const userName = userMap.get(task.userId) || 'Unknown user';

            // Создание элемента задачи
            const taskItem = document.createElement('div');
            taskItem.className = 'task-item';

            taskItem.innerHTML = `
                <h3>${task.title}</h3>
                <p><strong>Description:</strong> ${task.description || 'No description'}</p>
                <p><strong>Created Time:</strong> ${createdTime}</p>
                <p><strong>Last Update Time:</strong> ${lastUpdateTime}</p>
                <p><strong>Tags:</strong> ${tags}</p>
                <p><strong>Completed:</strong> ${task.isCompleted ? 'Yes' : 'No'}</p>
                <p><strong>User:</strong> ${userName}</p> <!-- Имя пользователя -->
                <button class="delete-btn" data-id="${task.id}">Delete</button>
            `;
            
            taskList.appendChild(taskItem);
        });

        document.querySelectorAll('.delete-btn').forEach(button => {
            button.addEventListener('click', handleDeleteTask);
        });
    } catch (error) {
        console.error('Error loading tasks:', error);
        alert('Error loading tasks');
    }
}

// Handle task edit
async function handleEditTask(event) {
    const taskId = event.target.dataset.id;
    // Загрузить текущие данные задачи и предоставить форму для редактирования
    // Можно использовать fetch для получения задачи по ID и заполнять форму
    const response = await fetch(`http://localhost:5153/api/tasks/${taskId}`);
    const task = await response.json();
    
    // Заполнить форму редактирования (примеры значений)
    document.getElementById('taskTitle').value = task.title;
    document.getElementById('taskDescription').value = task.description;
    document.getElementById('taskTags').value = task.tags.map(tag => tag.id);
    
    // Удалить старую задачу и добавить новую
    // Сохранение изменений будет аналогично добавлению новой задачи
}

// Handle task delete
async function handleDeleteTask(event) {
    const taskId = event.target.dataset.id;
    const response = await fetch(`http://localhost:5153/api/tasks/${taskId}`, {
        method: 'DELETE'
    });

    if (response.ok) {
        alert('Task deleted successfully');
        loadTasks(); // Перезагрузить список задач
    } else {
        alert('Error deleting task');
    }
}

document.getElementById('getAllTasks').addEventListener('click', loadTasks);



// Fetch and populate tags dropdown
async function populateTagsDropdown() {
    try {
        const response = await fetch('http://localhost:5153/api/tags');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const tags = await response.json();
        console.log('Tags fetched:', tags); // Добавлено для отладки
        const taskTagsSelect = document.getElementById('taskTags');
        
        // Очистить существующие опции
        taskTagsSelect.innerHTML = '';

        tags.forEach(tag => {
            const option = document.createElement('option');
            option.value = tag.id;
            option.textContent = tag.name;
            taskTagsSelect.appendChild(option);
        });
    } catch (error) {
        console.error('Error fetching tags:', error);
    }
}

let currentUserId = null;

// Fetch the current user info
async function getCurrentUser() {
    //need auth logic
    currentUserId = 1;
}

// Call this function when the page loads
document.addEventListener('DOMContentLoaded', async () => {
    await populateTagsDropdown();
    await getCurrentUser();
    await loadTags();
});
