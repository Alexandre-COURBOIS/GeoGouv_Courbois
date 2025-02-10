import { getVilles } from "../services/villeService.js";
import { debounce } from "../services/debouncerService.js";
import { renderTable } from "../compoments/dynamicTableRender.js";

const villePerPage = 50;
let currentPage = 1;

let isLoading = false;
let allVilles = [];
let displayedVilles = [];

let search = "";

/*Permet de temporiser entre chaque frappes de l'utilisateur et de sotcker si des options de filtre / trie ont été appliquées et d'afficher les villes correspondantes*/
const filterTable = debounce(() => {
    search = document.getElementById("search").value.trim();

    let ascending = sessionStorage.getItem("ascending");
    let sortBy = sessionStorage.getItem("sortBy");

    getVilleToDisplay(true, sortBy, ascending);
}, 300);

/*Permet d'afficher les villes dans le tableau, cette fonction et appelé à chaque scroll et s'incrémente*/
async function getVilleToDisplay(reset = false, sortBy, ascending) {
    if (isLoading) return;
    isLoading = true;
    document.getElementById("loading").style.display = "block";
    console.log(reset, sortBy, ascending);
    try {
        if (reset) {
            currentPage = 1;
            allVilles = [];
            displayedVilles = [];
            document.getElementById("ville-table-body").innerHTML = "";
        }

        const villes = await getVilles(currentPage, villePerPage, sortBy, ascending, search);

        console.log(villes);

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
/*Retoune la valeur de isLoading pour savoir si oui ou non je suis en chargement de valeur*/
function getIsLoading() {
    return isLoading;
}

export { getVilleToDisplay, filterTable, getIsLoading };
