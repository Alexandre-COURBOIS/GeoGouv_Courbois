import { getCommunes } from "../services/communeService.js";
import { debounce } from "../services/debouncerService.js";
import { renderTable } from "../compoments/dynamicTableRender.js";

const communePerPage = 50;

let currentPage = 1;

let isLoading = false;
let allCommunes = [];
let displayedCommunes = [];

let search = "";

const filterTable = debounce(() => {
    search = document.getElementById("search").value.trim();

    let ascending = sessionStorage.getItem("ascending");
    let sortBy = sessionStorage.getItem("sortBy");

    getCommunesToDisplay(true, sortBy, ascending);
}, 300);


async function getCommunesToDisplay(reset = false, sortBy, ascending) {
    if (isLoading) return;
    isLoading = true;
    document.getElementById("loading").style.display = "block";

    try {
        if (reset) {
            currentPage = 1;
            allCommunes = [];
            displayedCommunes = [];
            document.getElementById("commune-table-body").innerHTML = "";
        }

        const communes = await getCommunes(currentPage, communePerPage, sortBy, ascending, search);

        allCommunes.push(...communes);
        displayedCommunes = [...allCommunes];

        renderTable(displayedCommunes);
        currentPage++;

    } catch (error) {
        console.error("Erreur au chargement des communes", error);
    } finally {
        isLoading = false;
        document.getElementById("loading").style.display = "none";
    }
}
function getIsLoading() {
    return isLoading;
}

export { getCommunesToDisplay, filterTable, getIsLoading };
