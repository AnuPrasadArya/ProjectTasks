function getToken() {
    let tokenClaim = document.cookie.match('(^|;)\\s*'.concat('.AspNet.MyCookieAuth', '\\s*=\\s*([^;]+)'));
    if (tokenClaim) return tokenClaim.pop();
    return "";
}

function getAccessToken() {
    let value = document.cookie.split('; ').find(row => row.startsWith('.AspNet.MyCookieAuth='));
    if (!value) return null;

    let accessToken = "";
    try {
        let claims = atob(value.split('=')[1].split('.')[1]);
        accessToken = JSON.parse(claims)["AccessToken"];
    } catch {
        return null;
    }
    return accessToken;
}

function showProjectForm() {
    document.getElementById("projectForm").style.display = "block";
}

function showTaskForm() {
    document.getElementById("taskForm").style.display = "block";
}

async function loadProjects() {
    let token = getAccessToken();
    const res = await fetch('/Account/Login');
    if (!token) return;

    const response = await fetch('https://localhost:5001/api/Projects', {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    const projects = await response.json();
    let list = document.getElementById("projectsList");
    list.innerHTML = "";
    projects.forEach(p => {
        list.innerHTML += `<li class="list-group-item">
            ${p.name}
            <a href="/Home/Tasks?projectId=${p.id}" class="btn btn-sm btn-primary ms-2">Tasks</a>
        </li>`;
    });
}

async function createProject() {
    let token = getAccessToken();
    let data = {
        name: document.getElementById("projectName").value,
        description: document.getElementById("projectDesc").value
    };
    await fetch('https://localhost:5001/api/Projects', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    loadProjects();
}

async function loadTasks(projectId) {
    let token = getAccessToken();
    const response = await fetch(`https://localhost:5001/api/projects/${projectId}/tasks`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    const tasks = await response.json();
    let list = document.getElementById("tasksList");
    list.innerHTML = "";
    tasks.forEach(t => {
        list.innerHTML += `<li class="list-group-item">
            ${t.title} - Due: ${t.dueDate.split('T')[0]} - Completed: ${t.isCompleted}
        </li>`;
    });
}

async function createTask(projectId) {
    let token = getAccessToken();
    let data = {
        title: document.getElementById("taskTitle").value,
        description: document.getElementById("taskDesc").value,
        dueDate: document.getElementById("taskDueDate").value,
        isCompleted: false
    };
    await fetch(`https://localhost:5001/api/projects/${projectId}/tasks`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    loadTasks(projectId);
}
