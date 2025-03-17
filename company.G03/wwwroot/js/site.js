document.addEventListener("DOMContentLoaded", function () {
    let searchInput = document.getElementById("searchInput");
    let tableRows = document.querySelectorAll("table tbody tr");

    searchInput.addEventListener("keyup", function () {
        let searchText = searchInput.value.toLowerCase();

        tableRows.forEach(row => {
            let nameCell = row.querySelector("td:first-child");
            if (nameCell) {
                let nameText = nameCell.textContent.toLowerCase();
                row.style.display = nameText.includes(searchText) ? "" : "none";
            }
        });
    });
});
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
