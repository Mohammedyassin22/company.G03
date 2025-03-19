document.addEventListener("DOMContentLoaded", function () {
    const darkModeToggle = document.getElementById("toggleDarkMode");
    const body = document.body;

    // التحقق مما إذا كان المستخدم قد فعّل الوضع الداكن مسبقًا
    if (localStorage.getItem("dark-mode") === "enabled") {
        body.classList.add("dark-mode");
    }

    darkModeToggle.addEventListener("click", function () {
        body.classList.toggle("dark-mode");

        // حفظ الحالة في Local Storage
        if (body.classList.contains("dark-mode")) {
            localStorage.setItem("dark-mode", "enabled");
        } else {
            localStorage.setItem("dark-mode", "disabled");
        }
    });
});