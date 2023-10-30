const addCardbtn = document.querySelectorAll('.add_card_btn')


addCardbtn.forEach((elem) => {
    elem.addEventListener('click', () => {
        const idStage = elem.parentElement.getAttribute('idstage')
        createCard(elem)
    })
})


function closeCard(elem) {
    const elemTodelete = elem.parentElement.parentElement;
    elemTodelete.remove()
}


function createCard(elem) {
    console.log(elem.previousElementSibling)
    let div = document.createElement('div')


    $.ajax({
        type: "GET",
        url: 'GetCreateCardViewComponent',
        success: function (result) {
            div.innerHTML = result;
            elem.previousElementSibling.appendChild(div)
            elem.previousElementSibling.scrollTop
        },
    });
}