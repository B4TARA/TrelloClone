const openPopupButtons = document.querySelectorAll('.open-popup');
const openPopupAddButtons = document.querySelectorAll('.open-popup-add');
const mainContainers = document.getElementById('main_container_content');



openPopupButtons.forEach((elem) => {
    
    elem.addEventListener('click', () => {
        const idCard = elem.getAttribute('idcard')
        editCard(idCard)
    })
})


openPopupAddButtons.forEach((elem) => {

    elem.addEventListener('click', () => {
        addCard()
        
    })    
})


function closeCard(elem) {
    const elemTodelete = elem.parentElement.parentElement.parentElement;
    elemTodelete.remove()
}


async function editCard(idCard) {

    let div = document.createElement('div')
    div.setAttribute("id", "cardDetails")
   
    await fetch('GetCardLayoutViewComponent?' + new URLSearchParams({
        cardId: idCard
    }), 
    {
        method: 'GET',
    })
        .then((response) => response.text())
        .then((data) => {
            div.innerHTML = data;
            mainContainers.appendChild(div)
        })

    const popupMainInfo = document.getElementById('info_popup_card_wrapper')

    await fetch('GetCardInfoViewComponent?' + new URLSearchParams({
        cardId: idCard
    }),
        {
            method: 'GET',
        })
        .then((response) => response.text())
        .then((data) => {
            let div = document.createElement('div');
            div.innerHTML = data;
            popupMainInfo.appendChild(div)
        })
}

async function assessmentCard(idCard) {

    const popupMainInfo = document.getElementById('info_popup_card_wrapper')

    await fetch('GetCardAssessmentViewComponent?' + new URLSearchParams({
        cardId: idCard
    }),
        {
            method: 'GET',
        })
        .then((response) => response.text())
        .then((data) => {
            popupMainInfo.innerHTML = data;
        })
}

async function historyCard(idCard) {

    const popupMainInfo = document.getElementById('info_popup_card_wrapper')

    await fetch('GetCardHistoryViewComponent?' + new URLSearchParams({
        cardId: idCard
    }),
        {
            method: 'GET',
        })
        .then((response) => response.text())
        .then((data) => {
            popupMainInfo.innerHTML = data;
        })
}

async function infoCard(idCard) {

    const popupMainInfo = document.getElementById('info_popup_card_wrapper')

    await fetch('GetCardInfoViewComponent?' + new URLSearchParams({
        cardId: idCard
    }),
        {
            method: 'GET',
        })
        .then((response) => response.text())
        .then((data) => {
            popupMainInfo.innerHTML = data;
        })
}

async function addCard() {

    let div = document.createElement('div')

    await fetch('AddCardViewComponent',
        {
            method: 'GET',
        })
        .then((response) => response.text())
        .then((data) => {
            div.innerHTML = data;
            mainContainers.appendChild(div)
        })
}







