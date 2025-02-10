import { getVilleToDisplay, getIsLoading } from "../pages/villePage.js";

/*Methode permettant de gérer de manière lazy le chargement des données dès que mon scroll arrive en bas de page*/
export function setupLazyLoaderOnScroll() {
    window.addEventListener('scroll', () => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 100 && !getIsLoading()) {

            var ascending = sessionStorage.getItem("ascending", ascending);
            var sortBy = sessionStorage.getItem("sortBy", sortBy);

            getVilleToDisplay(false, sortBy, ascending);
        }
    });
}
