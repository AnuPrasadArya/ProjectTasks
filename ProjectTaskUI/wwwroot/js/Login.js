document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch("https://localhost:7125/api/Login/Login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        const data = await response.json();
        console.log("data", data);
        localStorage.setItem("accessToken", data.token);
        localStorage.setItem("userId", data.userId);
        window.location.href = "/Home/Projects";
    } else {
        document.getElementById("loginError").innerText = "Invalid username or password.";
        document.getElementById("loginError").style.display = "block";
    }
});
