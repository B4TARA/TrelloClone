const openPopupButtons = document.querySelectorAll('.open-popup');
const mainContainers = document.getElementById('main_container_content');


openPopupButtons.forEach((elem) => {
    
    elem.addEventListener('click', () => {
        const idCard = elem.getAttribute('idcard')
        editCard(idCard)
    })
})


function closeCard(elem) {
    const elemTodelete = elem.parentElement.parentElement;
    elemTodelete.remove()
}


async function editCard(idCard) {
    console.log(typeof(idCard))
    let dataToSend = {};
    dataToSend.idCard

    let div = document.createElement('div')
   
    await fetch('GetCardDetailsViewComponent?' + new URLSearchParams({
        cardId: idCard,
    }), {
        method: 'GET',
    })
        .then((response) => response.text())
        .then((data) => {
            //console.log(data)
            div.innerHTML = data;
            mainContainers.appendChild(div)
        })
}






