const addCardbtn = document.querySelectorAll('.add_card_btn')

addCardbtn.forEach((elem)=>{
    elem.addEventListener('click',()=>{
        const idStage = elem.parentElement.getAttribute('idstage')
        createCard(elem)
    })
})


function closeCard(elem){
    const elemTodelete = elem.parentElement.parentElement;
    elemTodelete.remove()
}

function createCard(elem){
    console.log(elem.previousElementSibling)
    let div = document.createElement('div')
        div.innerHTML = `<div class="card_wrapper open-popup">
        <div>
            <textarea name="" id=""placeholder = "Введите заголовок для этой карточки" class="edit_description_input"></textarea>
        </div>
        </div>
        <div class="action_buttons_wrapper">
            <div class="action_btn primary_btn" id="saveTextArea" onclick="addCard(this)">
                <div class="description">Добавить карточку</div>
            </div>
            <div class="action_btn close_action_btn white_btn" id="saveTextArea" onclick="closeCard(this)">
            <div>
                <i class="fa-solid fa-xmark" style="color: #000000;"></i>
            </div>
            </div>

        </div>
        
        `
        elem.previousElementSibling.appendChild(div)
        elem.previousElementSibling.scrollTop
}

function addCard(elem){
    console.log()
    // createCard(elem.parentElement.parentElement.parentElement)
}