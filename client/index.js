"use strict";
const fetchPeople = async (expression) => {
    let url = "http://localhost:5001";
    if (expression) {
        console.log(expression);
        url += `?expression=${encodeURIComponent(expression)}`;
    }
    const res = await fetch(url);
    return res.json();
};
const fillTable = async (people) => {
    const table = document.getElementById("people-table-body");
    if (table == null)
        return;
    while (table.firstChild) {
        table.removeChild(table.firstChild);
    }
    people.forEach((p) => {
        const row = document.createElement("tr");
        const firstNameCell = document.createElement("td");
        firstNameCell.textContent = p.fName;
        row.appendChild(firstNameCell);
        const lastNameCell = document.createElement("td");
        lastNameCell.textContent = p.lName;
        row.appendChild(lastNameCell);
        const dobCell = document.createElement("td");
        dobCell.textContent = p.dob.toString();
        row.appendChild(dobCell);
        table.appendChild(row);
    });
};
const updateTable = async () => {
    const expressionText = document.getElementById("expression-text");
    const people = await fetchPeople(expressionText.value);
    fillTable(people);
};
document.addEventListener("DOMContentLoaded", async () => {
    const people = await fetchPeople();
    fillTable(people);
});
