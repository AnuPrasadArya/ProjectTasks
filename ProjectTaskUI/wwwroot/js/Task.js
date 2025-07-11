function showTaskForm() {
    document.getElementById("taskForm").style.display = "block";
}

async function createTask() {
    const projectId = document.getElementById("projectId").value;
    const title = document.getElementById("taskTitle").value;
    const description = document.getElementById("taskDesc").value;
    const dueDate = document.getElementById("taskDueDate").value;
    const completed = document.getElementById("taskCompleted").checked;

    const token = localStorage.getItem("accessToken");

    const response = await fetch(`https://localhost:7010/api/projects/${projectId}/tasks`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({
            title,
            description,
            dueDate,
            isCompleted: completed
        })
    });

    if (response.ok) {
        alert("Task created!");
        loadTasks();
    } else {
        alert("Error creating task.");
    }
}

async function loadTasks() {
    const projectId = document.getElementById("projectId").value;
    const token = localStorage.getItem("accessToken");

    const res = await fetch(`https://localhost:7010/api/projects/${projectId}/tasks`, {
        headers: { "Authorization": "Bearer " + token }
    });

    if (res.ok) {
        const tasks = await res.json();
        const list = document.getElementById("tasksList");
        list.innerHTML = "";
        tasks.forEach(t => {
            const li = document.createElement("li");
            li.className = "list-group-item d-flex justify-content-between align-items-center";

            li.innerHTML = `
                <div>
                    <strong>${t.title}</strong> <br/>
                    ${t.description} <br/>
                    Due: ${t.dueDate?.substring(0, 10) || "N/A"} <br/>
                    Completed: ${t.isCompleted ? "Yes" : "No"}
                </div>
                <div>
                    <button class="btn btn-sm btn-primary me-1" onclick="editTask(${t.id}, '${t.title}', '${t.description}', '${t.dueDate}', ${t.isCompleted})">Edit</button>
                    <button class="btn btn-sm btn-danger" onclick="deleteTask(${t.id})">Delete</button>
                </div>
            `;
            list.appendChild(li);
        });
    } else {
        alert("Error loading tasks.");
    }
}

async function deleteTask(taskId) {
    const projectId = document.getElementById("projectId").value;
    const token = localStorage.getItem("accessToken");

    const res = await fetch(`https://localhost:7010/api/projects/${projectId}/tasks/${taskId}`, {
        method: "DELETE",
        headers: { "Authorization": "Bearer " + token }
    });

    if (res.ok) {
        alert("Task deleted!");
        loadTasks();
    } else {
        alert("Error deleting task.");
    }
}

function editTask(taskId, title, description, dueDate, isCompleted) {
    document.getElementById("taskTitle").value = title;
    document.getElementById("taskDesc").value = description;
    document.getElementById("taskDueDate").value = dueDate ? dueDate.substring(0, 10) : "";
    document.getElementById("taskCompleted").checked = isCompleted;
    document.getElementById("taskForm").style.display = "block";

    // Replace Save button with Update
    const saveBtn = document.querySelector("#taskForm button");
    saveBtn.textContent = "Update";
    saveBtn.onclick = function () {
        updateTask(taskId);
    };
}

async function updateTask(taskId) {
    const projectId = document.getElementById("projectId").value;
    const title = document.getElementById("taskTitle").value;
    const description = document.getElementById("taskDesc").value;
    const dueDate = document.getElementById("taskDueDate").value;
    const completed = document.getElementById("taskCompleted").checked;

    const token = localStorage.getItem("accessToken");

    const response = await fetch(`https://localhost:7010/api/projects/${projectId}/tasks/${taskId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({
            title,
            description,
            dueDate,
            isCompleted: completed
        })
    });

    if (response.ok) {
        alert("Task updated!");
        document.querySelector("#taskForm button").textContent = "Save";
        document.querySelector("#taskForm button").onclick = createTask;
        loadTasks();
    } else {
        alert("Error updating task.");
    }
}
