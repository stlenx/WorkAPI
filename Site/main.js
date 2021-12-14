SetToolTipStyle("white", "black");

const Http = new XMLHttpRequest();
const url='http://192.0.4.80:5001/manageSites';
const SitesContainer = document.getElementById("sites");

Http.onreadystatechange = () => {
    if (Http.readyState !== 4 || Http.status !== 200) {
        if(Http.status === 409) {
            //Conflict, page already exists
            displayErrorMessage(
                "La página ya existe",
                "#ffffff",
                "#ff3d3d",
                2500)
        }

        return;
    } // Check for ready because xmlhttprequest gae

    if(Http.responseText === "") return

    let output = JSON.parse(Http.responseText)
    console.log(output)

    //Remove any previous site that was there
    while (SitesContainer.hasChildNodes()) SitesContainer.removeChild(SitesContainer.lastChild)

    for (const [key, value] of Object.entries(output.sites)) {

        let site = document.createElement("a");
        site.innerHTML = key;
        site.href = value.url;
        site.target = "_blank";

        SitesContainer.appendChild(site);
    }
}

function getListOfSites() {
    Http.open("GET", url);
    Http.setRequestHeader("accept", "text/plain");
    Http.send();
}

function AddSite() {
    let newName = document.getElementById("newSiteName").value;
    let newUrl = document.getElementById("newSiteUrl").value;
    let query = document.getElementById("newSiteQuery").value;
    let resultsContainerType = document.getElementById("newSiteResultsContainerType").value;
    let resultsContainerData = document.getElementById("newSiteResultsContainerData").value;
    let resultName = document.getElementById("newSiteResultName").value;
    let resultPrice = document.getElementById("newSiteResultPrice").value;
    let resultImage = document.getElementById("newSiteResultImage").value;
    let resultLink = document.getElementById("newSiteResultLink").value;

    Http.open("POST", url);
    Http.setRequestHeader("Content-Type", "application/json")
    Http.send(JSON.stringify({
        "name": newName,
        "url": newUrl,
        "query": query,
        "resultsContainer": {
            "type": resultsContainerType,
            "data": resultsContainerData
        },
        "resultName": resultName,
        "resultPrice": resultPrice,
        "resultImage": resultImage,
        "resultLink": resultLink
    }));

    HideAddSitePage();

    setTimeout(function () {
        getListOfSites();
    }, 500)
}

getListOfSites();