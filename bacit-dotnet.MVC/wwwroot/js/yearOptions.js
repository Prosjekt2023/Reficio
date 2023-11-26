const currentYear = new Date().getFullYear();
for (let year = 1950; year <= currentYear; year++) {
    const option = document.createElement("option");
    option.value = year;
    option.textContent = year;
    document.getElementById("Year").appendChild(option);
}
