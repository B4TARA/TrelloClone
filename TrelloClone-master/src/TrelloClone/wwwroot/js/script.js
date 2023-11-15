
//SIDEBAR CODE

const arrowShowMenu = document.querySelectorAll(".arrow"),
    sidebar = document.querySelector(".sidebar"),
    sidebarBtn = document.querySelector(".sidebar_close_btn_wrapper");


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
    let optionsContainer = elem.previousElementSibling;
    optionsContainer.classList.toggle("active");
    elem.classList.toggle("active_border");
}

function optionClick(elem) {
    console.log(elem)
    const resultAssessmentWrapper = elem.parentElement.nextElementSibling.querySelector('.input_assessment_value');
    const descriptionAssessmentValElem = elem.parentElement.nextElementSibling.querySelector('.value_asessessment');

    //Открытие закрытие дива с селектом
    const optionsContainer = elem.parentElement;
    optionsContainer.nextElementSibling.classList.toggle("active_border");
    optionsContainer.classList.toggle("active");

    
    const resultAssessmentText = elem.querySelector(".select_user_assessment").innerText
    console.log(resultAssessmentText)
    descriptionAssessmentValElem.innerHTML = resultAssessmentText;

    const resultAssessmentVal = elem.querySelector(".select_user_assessment").getAttribute('itemid')
    resultAssessmentWrapper.value = resultAssessmentVal;
}
