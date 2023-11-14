function addCommentArea(elem) {
    console.log(elem.nextElementSibling)
    elem.nextElementSibling.classList.toggle("active");
}

function closeTextEditor() {
    const saveBtn = document.getElementById('addCommentBtns')
    saveBtn.classList.toggle("active");
}

function saveComment(elem) {
    const saveBtn = document.getElementById('addCommentBtns')
    saveBtn.classList.toggle("active");
    const commentAreaElem = document.getElementById('commentAreaElem')

    const commentAreaMainWrapper = document.getElementById('comment_popup_card_wrapper')
    if (commentAreaElem.value != "") {
        let div = document.createElement('div')
        div.classList.add('comment_user_wrapper')
        //div.classList.add('first_row_grid_description');
        //div.classList.add('margin_container_top_middle');

        div.innerHTML = `<div class="first_row_grid_description margin_container_top_middle">
        <div class="card_holders_wrapper">
    <div class="holder_image_wrapper">
        <img src="/image/user_image/ПоляковаИннаЮлиановна31011971.jpeg" alt="holder_image">
    </div>
    </div>
    <div class="mid_title">
         Полякова Инна
    </div>
        </div>
        

    <div class="second_row_grid_description" id="description">
         <div></div>
         <div class="commentarea_wrapper" id="commentarea_wrapper">
            <div class="comment_area">
                <div class="mid_description">${commentAreaElem.value}</div>
            </div>
        </div>
    </div>
    `

        commentAreaMainWrapper.appendChild(div)
    }
    //commentAreaElem.value = ""
}

