const openPopupButtons = document.querySelectorAll('.open-popup');
const mainContainers = document.querySelectorAll('.main_container_content');


openPopupButtons.forEach((elem) => {
    elem.addEventListener('click', () => {
        editCard()
    })
})


function editCard() {


    let div = document.createElement('div')


    $.ajax({
        type: "GET",
        url: 'GetEditCardViewComponent',
        success: function (result) {
            div.innerHTML = result;
            mainContainers[0].appendChild(div)
            mainContainers[0].scrollTop
        }
    });
}