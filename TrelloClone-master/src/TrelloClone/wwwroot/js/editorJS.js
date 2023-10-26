
var editorInstance;
const descriptionWrapper = document.getElementById('description')
const textareaElement = document.getElementById('editor')
const placeholder = document.getElementById('placeholder');
const editor = document.getElementById('editor');


function createTextEditor (elem){
    let eventTargetElem = elem.getAttribute('id')

    editorInstance = CKEDITOR.replace(`${eventTargetElem}`,{
        resize_enabled: false,
        updateSourceElementOnDestroy: true,
    });

    let div = document.createElement('div')
    div.classList.add('second_row_grid_description')
    div.classList.add('add_texteditor_buttons_wrapper')
    div.setAttribute('id','saveTextAreaWrapper')
    div.innerHTML = `
    <div></div>
    <div class="action_buttons_wrapper">
                <div class="action_btn primary_btn" id="saveTextArea" onclick="saveTextArea(${eventTargetElem})">
                    <div class="description">Сохранить</div>
                </div>
                <div class="action_btn close_action_btn white_btn" id="saveTextArea" onclick="closeTextEditor()">
                <div>
                    <i class="fa-solid fa-xmark" style="color: #000000;"></i>
                </div>
                </div>

            </div>
</div>`
    
elem.parentElement.appendChild(div)
}

function saveTextArea(elem){
    let elemId = elem.getAttribute('id')
    const editorData = editorInstance.getData()
    if(elemId == "comment_editor"){
        saveTextAreaDescription(editorData)
    }
    const saveBtn = document.getElementById('saveTextAreaWrapper')
    saveBtn.remove()
    editorInstance.destroy();
    editorInstance = null;
}

function saveTextAreaDescription (data){
    const commentAreaMainWrapper = document.getElementById('comment_popup_card_wrapper')
    let div = document.createElement('div')
    div.classList.add('comment_user_wrapper')
    div.classList.add('second_row_grid_description')
    div.classList.add('margin_container_bottom_middle')
    div.innerHTML = `<div class="card_holders_wrapper">
    <div class="holder_image_wrapper">
        <img src="profile.jpg" alt="holder_image">
    </div>
    </div>
    <div class="commentarea_wrapper" id="commentarea_wrapper">
    <div class="comment_area">
    <div class="mid_description">${data}</div>
    </div>
    </div>`
commentAreaMainWrapper.appendChild(div)

}

function closeTextEditor (){
    const saveBtn = document.getElementById('saveTextAreaWrapper')
    saveBtn.remove()
    editorInstance.destroy();
    editorInstance = null;
}

