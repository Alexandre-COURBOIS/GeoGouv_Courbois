import { getVilleToDisplay, filterTable } from "./pages/villePage.js";
import { setupLazyLoaderOnScroll } from "./utils/lazyLoader.js";

let sortBy = "nom"; 
let ascending = "asc";

document.addEventListener("DOMContentLoaded", () => {

    const searchInput = document.getElementById("search");
    const tableHeaders = document.querySelectorAll("th[data-sort]");

    if (searchInput) {
        searchInput.addEventListener("keyup", () => {
            filterTable();
        });
    } else {
        console.error("Erreur : impossible de trouver l'élément search dans le DOM");
    }

    tableHeaders.forEach(header => {
        header.addEventListener("click", () => {
            const clickedColumn = header.getAttribute("data-sort");

            if (clickedColumn) {
          
                if (sortBy === clickedColumn) {
                    ascending = !ascending;
                    sessionStorage.setItem("ascending", ascending);
                    sessionStorage.setItem("sortBy", sortBy);

                } else {
                    sortBy = clickedColumn;   
                    sessionStorage.setItem("ascending", ascending);
                    sessionStorage.setItem("sortBy", sortBy);
                }
                getVilleToDisplay(true, sortBy, ascending); 
            }
        });
    });

    getVilleToDisplay(true, sortBy, true);
    setupLazyLoaderOnScroll();
});
