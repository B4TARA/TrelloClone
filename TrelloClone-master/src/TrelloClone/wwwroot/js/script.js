
    //SIDEBAR CODE

    const arrowShowMenu = document.querySelectorAll(".arrow"),
    sidebar = document.querySelector(".sidebar"),
    sidebarBtn = document.querySelector(".bx-menu");


arrowShowMenu.forEach(item => {
    item.addEventListener("click", (e) => {
        let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
        arrowParent.classList.toggle("showMenu");
    });
})

sidebarBtn.addEventListener("click", () => {
    sidebar.classList.toggle("close_sidebar");
});


var searchBoxItem = document.getElementById("selectBox");


function selectedContainerOpen(elem) {
    let optionsContainer = document.getElementById("options-container");

    optionsContainer.classList.toggle("active");
    elem.classList.toggle("active_border");
}

function optionClick(elem) {
    const resultAssessmentWrapper = document.getElementById('valueAsessessment');
    const descriptionAssessmentValElem = document.getElementById('descriptionAssessmentVal');

    const optionsContainer = document.getElementById("options-container");
    optionsContainer.nextElementSibling.classList.toggle("active_border");
    optionsContainer.classList.toggle("active");

    const resultAssessmentText = elem.querySelector("label").innerText
    descriptionAssessmentValElem.innerHTML = resultAssessmentText;

    const resultAssessmentVal = elem.querySelector("label").getAttribute('itemval')
    console.log(resultAssessmentVal)
    resultAssessmentWrapper.value = resultAssessmentVal;
}