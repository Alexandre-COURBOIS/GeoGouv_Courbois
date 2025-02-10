import { getVilleToDisplay, filterTable } from "./pages/villePage.js";
import { setupLazyLoaderOnScroll } from "./utils/lazyLoader.js";

let sortBy = "nom"; 
let ascending = false;

/*Ajoute des évènements de déclenchage lorsque le DOM est chargé*/
document.addEventListener("DOMContentLoaded", () => {

    const searchInput = document.getElementById("search");
    const tableHeaders = document.querySelectorAll("th[data-sort]");

    /*Lorsque l'utilisateur fait une saisie dans la barre de recherche j'enclenche le filtre d'affichage du tableau*/
    if (searchInput) {
        searchInput.addEventListener("keyup", () => {
            filterTable();
        });
    } else {
        console.error("Error : Impossible to find this element in the DOM");
    }

    /*Gestion du trie lors du click sur une colonne et affichage en fonction*/    
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
    /*Affichage par défaut*/
    getVilleToDisplay(true, sortBy, true);

    /*Mise en place du lazy loader lors des scrolls*/
    setupLazyLoaderOnScroll();
});
