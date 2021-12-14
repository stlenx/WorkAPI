const Http = new XMLHttpRequest();
const ProfilesRequest = new XMLHttpRequest();

const domain = "http://192.0.4.80:5001";
let searchUrl = `${domain}/search/cache`;
const listUrl = `${domain}/manageSites`;
const profileUrl = `${domain}/profiles`;

let sites = {};
let allSites = [];
let profiles = {};
let cachedTimes = {};

Http.onreadystatechange = () => {
    if (Http.readyState !== 4 || Http.status !== 200) {

        return;
    } // Check for ready because xmlhttprequest gae

    let output = JSON.parse(Http.responseText);

    for (const [key] of Object.entries(output.sites)) {
        let client = new XMLHttpRequest();

        client.onreadystatechange = () => {
            processResponse(client);
        }

        sites[key] = client;

        allSites.push(key);

        AddSiteToConfig(key);
    }

    if(Object.keys(sites).length === 0) {
        removeLoading();
        displayErrorMessage(
            "No hay ninguna página donde buscar",
            "#ffffff",
            "#ff3d3d",
            2500
        )
    }
}

ProfilesRequest.onreadystatechange = () => {
    if (ProfilesRequest.readyState !== 4 || ProfilesRequest.status !== 200) {
        return;
    } // Check for ready because xmlhttprequest gae

    let output = JSON.parse(ProfilesRequest.responseText);
    profiles = output;

    let select = document.getElementById("profileSelector");

    //remove all previous trash
    while (select.hasChildNodes()) select.removeChild(select.lastChild);

    //Add unselector
    addProfileToList("ninguno", -1);

    //Add all profiles
    for (const [key] of Object.entries(output)) {
        addProfileToList(key, key);
    }
}

function search() {
    stopTimers();
    removeResults();
    addLoading();
    let item = document.getElementById("search").value;
    item = encodeURIComponent(item);

    if(Object.keys(sites).length === 0) {
        removeLoading();
        displayErrorMessage(
            "No hay ninguna página donde buscar",
            "#ffffff",
            "#ff3d3d",
            2500
        )
        return;
    }

    for (const [site, client] of Object.entries(sites)) {
        //http://192.+0.4.80:5001/search/signs/penguin

        console.log(site, `${searchUrl}/${site}/${item}`);

        client.open("GET", `${searchUrl}/${site}/${item}`);
        client.setRequestHeader("Content-Type", "application/json");
        client.send();
    }
}

function AddSiteToConfig(site) {
    let container = document.getElementById("sitesGoHere");

    let superDiv = document.createElement("div");
    let label = document.createElement("label");
    let input = document.createElement("input");

    label.innerText = site;

    input.type = "checkbox";
    input.id = site;
    input.checked = true;
    input.onclick = function () {
        if(sites[this.id] !== undefined) {
            delete sites[this.id]
            return;
        }
        let client = new XMLHttpRequest();

        client.onreadystatechange = () => {
            processResponse(client);
        }

        sites[this.id] = client;
    }

    label.appendChild(input);

    superDiv.appendChild(label);

    container.appendChild(superDiv);
}

function removeResults() {
    let results = document.getElementById("results");
    while (results.hasChildNodes()) results.removeChild(results.lastChild);
}

function getSites() {
    Http.open("GET", listUrl);
    Http.setRequestHeader("accept", "text/plain");
    Http.send();
}

function getProfiles() {
    ProfilesRequest.open("GET", profileUrl);
    ProfilesRequest.setRequestHeader("accept", "text/plain");
    ProfilesRequest.send();
}

function changeProfile(profile) {
    let sites = profiles[profile];

    allSites.forEach((site) => {
        let input = document.getElementById(site);

        if((sites.indexOf(site) === -1) === input.checked) {
            input.click();
        }
    })
}

function addProfileToList(text, value) {
    let select = document.getElementById("profileSelector");

    let option = document.createElement("option");

    option.innerText = text;
    option.value = value;

    select.appendChild(option);
}

function addLoading() {
    document.getElementById("loading").style.display = "block";
}

function removeLoading() {
    document.getElementById("loading").style.display = "none";
}

function processResponse(Http) {
    if (Http.readyState !== 4 || Http.status !== 200) {
        return;
    } // Check for ready because xmlhttprequest gae

    let output = JSON.parse(Http.responseText);

    //Add result
    let mainContainer = document.createElement("a");
    mainContainer.classList.add("flexbox");
    mainContainer.classList.add("noselect");
    mainContainer.classList.add("result");

    mainContainer.href = output.result.link;
    mainContainer.target = "_blank";

    let siteName = document.createElement("div");
    siteName.classList.add("resultDetail");
    siteName.classList.add("siteName");
    siteName.innerText = output.site;

    let itemImage = document.createElement("img");
    itemImage.classList.add("resultDetail");
    itemImage.classList.add("itemImage");
    itemImage.src = output.result.image;

    let itemName = document.createElement("div");
    itemName.classList.add("resultDetail");
    itemName.classList.add("itemName");
    itemName.innerText = output.result.name;

    let itemPrice = document.createElement("div");
    itemPrice.classList.add("resultDetail");
    itemPrice.classList.add("itemPrice");
    itemPrice.innerText = output.result.price;

    let cached = document.createElement("div");
    cached.classList.add("resultDetail");
    cached.classList.add("resultCached");

    let cacheTime = document.createElement("div");
    cacheTime.classList.add("resultDetail");
    cacheTime.classList.add("cacheTime");
    cacheTime.id = `${output.site}-time`;

    if(output.cached) {
        cached.innerText = "por caché";
        cacheTime.innerText = SecondsToString(output.time);
        cachedTimes[output.site] = output.time;
    }

    mainContainer.appendChild(siteName);
    mainContainer.appendChild(itemImage);
    mainContainer.appendChild(itemName);
    mainContainer.appendChild(itemPrice);
    mainContainer.appendChild(cached);
    mainContainer.appendChild(cacheTime);

    let results = document.getElementById("results");

    results.appendChild(mainContainer);

    if(output.cached) {
        UpdateTime(output.site);
    }

    if(Object.keys(sites).length === results.childElementCount) {
        removeLoading();
    }
}

function UpdateTime(site) {
    if(cachedTimes[site] === undefined) {
        return;
    }
    
    cachedTimes[site] += 1;

    document.getElementById(`${site}-time`).innerText = SecondsToString(cachedTimes[site]);

    setTimeout(function () {
        UpdateTime(site)
    }, 1000)
}

function stopTimers() {
    cachedTimes = {};
}

function SecondsToString(input) {
    let sec_num = parseInt(input, 10); // don't forget the second param
    let days = Math.floor(sec_num / 86400);
    let hours   = Math.floor((sec_num - (days * 86400)) / 3600);
    let minutes = Math.floor((sec_num - (days * 86400) - (hours * 3600)) / 60);
    let seconds = sec_num - (days * 86400) - (hours * 3600) - (minutes * 60);

    let final = "";

    if(days > 0) {
        final += days + "d ";
    }

    if(hours > 0) {
        final += hours + "h ";
    }

    if(minutes > 0) {
        final += minutes + "m ";
    }

    if(seconds > 0) {
        final += seconds + "s";
    }

    if(final === "") {
        final += seconds + "s";
    }

    return final;
}

document.getElementById("cache").addEventListener("click", (e) => {
    if(e.target.checked) {
        searchUrl = `${domain}/search/cache`;
    } else {
        searchUrl = `${domain}/search/nocache`;
    }
})

document.getElementById("search").addEventListener("keyup", (e) => {
    if(e.code === "Enter") {
        search();
    }
})

document.getElementById("profileSelector").onchange = function() {
    let index = this.selectedIndex;
    let inputText = this.children[index].value;

    if(inputText === "-1") {
        return;
    }

    changeProfile(inputText);
}


getSites();
getProfiles();