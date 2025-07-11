$(document).ready(function () {
    loadProjectsDropdown();
});

async function loadProjectsDropdown() {
    const token = localStorage.getItem("accessToken");
    const userId = parseInt(localStorage.getItem("userId"));    
    const res = await fetch("https://localhost:7125/api/Project/GetProject", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ userId })
    });

    if (res.ok) {
        const projects = await res.json();

        const projectSelect = $("#projectSelect");
        const taskProjectSelect = $("#taskProjectSelect");
        projectSelect.empty().append(`<option value="">-- Select Project --</option>`);
        taskProjectSelect.empty().append(`<option value="">-- Select Project --</option>`);

        projects.forEach(p => {
            projectSelect.append(`<option value="${p.id}">${p.name}</option>`);
            taskProjectSelect.append(`<option value="${p.id}">${p.name}</option>`);
        });

    } else {
        alert("Error loading projects");
    }
}

function showTaskForm() {    
    $("#taskForm").show();
    $("#projectSelect").hide();
    $("#tasksTable").hide();
}

async function loadTasksForProject() {
    $("#projectSelect").show();
    const projectId = $("#projectSelect").val();
    const token = localStorage.getItem("accessToken");
    const userId = parseInt(localStorage.getItem("userId"));

    if (!projectId) {
        $("#tasksTable").hide();
        return;
    }

    const res = await fetch(`https://localhost:7125/api/Task/GetTasks`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ userId, projectId })
    });

    if (res.ok) {
        const tasks = await res.json();
        const body = $("#tasksBody");
        body.empty();

        tasks.forEach(task => {
            const row = `
                <tr>
                    <td>${task.id}</td>
                    <td><input type="text" class="form-control" id="title_${task.id}" value="${task.title}" /></td>
                    <td><textarea class="form-control" id="desc_${task.id}">${task.description}</textarea></td>
                    <td><input type="date" class="form-control" id="due_${task.id}" value="${task.dueDate.substring(0, 10)}" /></td>
                    <td><input type="checkbox" id="completed_${task.id}" ${task.isCompleted ? "checked" : ""} /></td>
                    <td>
                        <button class="btn btn-sm btn-primary" onclick="updateTask(${task.id})">Update</button>
                        <button class="btn btn-sm btn-danger" onclick="deleteTask(${task.id})">Delete</button>
                    </td>
                </tr>
            `;
            body.append(row);
        });

        $("#tasksTable").show();
    } else {
        alert("Error loading tasks");
    }
}

async function createTask() {
    const token = localStorage.getItem("accessToken");
    const userId = parseInt(localStorage.getItem("userId"));
    const projectId = $("#taskProjectSelect").val();
    const title = $("#taskTitle").val();
    const description = $("#taskDesc").val();
    const dueDate = $("#taskDueDate").val();
    const isCompleted = $("#taskCompleted").is(":checked");
    if (!projectId) {
        alert("Please select a project first!");
        return;
    }
    const res = await fetch("https://localhost:7125/api/Task/CreateTask", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({
            userId,
            projectId: parseInt(projectId),
            title,
            description,
            dueDate,
            isCompleted
        })
    });

    if (res.ok) {
        alert("Task created successfully!");
        $("#taskForm").hide();
        loadTasksForProject();
    } else {
        alert("Error creating task");
    }
}

async function updateTask(taskId) {
    const token = localStorage.getItem("accessToken");
    const userId = parseInt(localStorage.getItem("userId"));
    const title = $(`#title_${taskId}`).val();
    const description = $(`#desc_${taskId}`).val();
    const dueDate = $(`#due_${taskId}`).val();
    const isCompleted = $(`#completed_${taskId}`).is(":checked");

    const res = await fetch(`https://localhost:7125/api/Task/UpdateTask`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({
            taskId,
            title,
            description,
            dueDate,
            isCompleted,
            userId
        })
    });

    if (res.ok) {
        alert("Task updated successfully!");
        loadTasksForProject();
    } else {
        alert("Error updating task");
    }
}

async function deleteTask(taskId) {
    const token = localStorage.getItem("accessToken");
    const userId = parseInt(localStorage.getItem("userId"));
    if (!confirm("Are you sure you want to delete this task?")) return;

    const res = await fetch(`https://localhost:7125/api/Task/DeleteTask`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({
            taskId,            
            userId
        })
    });

    if (res.ok) {
        alert("Task deleted successfully!");
        loadTasksForProject();
    } else {
        alert("Error deleting task");
    }
}
