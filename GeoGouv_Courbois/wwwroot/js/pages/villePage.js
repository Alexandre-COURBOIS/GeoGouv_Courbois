import { getVilles } from "../services/villeService.js";
import { debounce } from "../services/debouncerService.js";
import { renderTable } from "../compoments/dynamicTableRender.js";

const villePerPage = 50;

let currentPage = 1;

let isLoading = false;
let allVilles = [];
let displayedVilles = [];

let search = "";

const filterTable = debounce(() => {
    search = document.getElementById("search").value.trim();

    let ascending = sessionStorage.getItem("ascending");
    let sortBy = sessionStorage.getItem("sortBy");

    getVilleToDisplay(true, sortBy, ascending);
}, 300);


async function getVilleToDisplay(reset = false, sortBy, ascending) {
    if (isLoading) return;
    isLoading = true;
    document.getElementById("loading").style.display = "block";

    try {
        if (reset) {
            currentPage = 1;
            allVilles = [];
            displayedVilles = [];
            document.getElementById("ville-table-body").innerHTML = "";
        }

        const villes = await getVilles(currentPage, villePerPage, sortBy, ascending, search);

        allVilles.push(...villes);
        displayedVilles = [...allVilles];

        renderTable(displayedVilles);
        currentPage++;

    } catch (error) {
        console.error("Erreur while loading citys", error);
    } finally {
        isLoading = false;
        document.getElementById("loading").style.display = "none";
    }
}

function getIsLoading() {
    return isLoading;
}

export { getVilleToDisplay, filterTable, getIsLoading };
