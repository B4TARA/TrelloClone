
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
    console.log(elem.parentElement.parentElement.nextElementSibling)

    //Отображение секции с комментом к оценке
    elem.parentElement.parentElement.nextElementSibling.classList.toggle('active')

    //Отображение секции с опшинами в селекте
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

    //Закрытие секции с комментом к оценке
    elem.parentElement.parentElement.parentElement.nextElementSibling.classList.toggle('active')
    
    const resultAssessmentText = elem.querySelector(".select_user_assessment").innerText
    console.log(resultAssessmentText)
    descriptionAssessmentValElem.innerHTML = resultAssessmentText;

    const resultAssessmentVal = elem.querySelector(".select_user_assessment").getAttribute('itemid')
    resultAssessmentWrapper.value = resultAssessmentVal;
}

function openDropdownList(elem) {
    const historyListWrapper = document.getElementById('history_list_wrapper')
    historyListWrapper.classList.toggle('active')
    elem.classList.toggle('active')
}

function openVerifyPopup(elem) {
    const mainContainerContentElement = document.getElementById('main_container_content');
    let cardId = elem.getAttribute('cardId')
    let div = document.createElement('div')
    div.classList.add('popup_verify_bg_wrapper')
    div.innerHTML = `<div class="popup_verify_wrapper">
    <div class="mid_title margin_container_bottom_middle">Вы действительно хотите удалить карточку?</div>
    <div class="action_buttons_wrapper">
        
            <div class="action_btn green_btn" onclick="sendInfoDeleteCard(${cardId})">
                Да
            </div>
        <div class="action_btn red_btn" onclick="closeVerifyPopup(this)">
            Нет
        </div>
    </div>
        
</div>`
    mainContainerContentElement.append(div)
}

function closeVerifyPopup(elem) {
    const popupElem = document.querySelector('.popup_verify_bg_wrapper')
    popupElem.remove()
}

function sendInfoDeleteCard(cardId) {
    
    var url = "/Card/Delete";
    formData = new FormData();
    formData.append("cardId", cardId);

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: async function () {
            window.location.href = "/UserBoard/ListMyCards";
        }
    });
}


