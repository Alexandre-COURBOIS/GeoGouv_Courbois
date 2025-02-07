export function renderTable(communeList) {
    const tableBody = document.getElementById("commune-table-body");
    tableBody.innerHTML = "";

    communeList.forEach(commune => {
        const row = `
            <tr>
                <td>${commune.nom || "Non communiqué"}</td>
                <td>${commune.code || "Non communiqué"}</td>
                <td>${commune.codeDepartement || "Non communiqué"}</td>
                <td>${commune.population?.toLocaleString() || "Non communiqué"}</td>
                <td>${commune.codeEpci || "Non communiqué"}</td>
                <td>${commune.codeRegion || "Non communiqué"}</td>
                <td>${(commune.codesPostaux || ["Non communiqué"]).join(", ") }</td>
            </tr>
        `;
        tableBody.insertAdjacentHTML('beforeend', row);
    });
}
