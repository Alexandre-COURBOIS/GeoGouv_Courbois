
import { fetchDetailsVille } from "../services/villeService.js";
export function renderTable(villeList) {
    const tableBody = document.getElementById("ville-table-body");
    tableBody.innerHTML = "";

    villeList.forEach(ville => {
        const row = `
            <tr>
                <td>${ville.nom || "Non communiqué"}</td>
                <td>${ville.code || "Non communiqué"}</td>
                <td>${ville.codeDepartement || "Non communiqué"}</td>
                <td>${ville.population?.toLocaleString() || "Non communiqué"}</td>
                <td>${ville.codeEpci || "Non communiqué"}</td>
                <td>${ville.codeRegion || "Non communiqué"}</td>
                <td>${(ville.codesPostaux || ["Non communiqué"]).join(", ") }</td>
                <td>
                    <button class="btn btn-success details-btn" data-code="${ville.code}">Insert BDD</button>
                </td>
            </tr>
        `;
        tableBody.insertAdjacentHTML('beforeend', row);
    });

    tableBody.querySelectorAll(".details-btn").forEach(button => {
        button.addEventListener("click", function () {
            const code = this.getAttribute("data-code");
            fetchDetailsVille(code);
        });
    });
}
