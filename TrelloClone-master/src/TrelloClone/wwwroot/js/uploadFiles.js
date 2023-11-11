const typeDoc = 'application/msword';
const typeDocx = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
const typePdf = 'application/pdf';
const typePng = 'image/png';
const typeJpeg = 'image/jpeg';
const typeJpg = 'image/jpg';
const typeExcel = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
const typeTxt = 'text/plain';

const objIconsFiles = [
    { typeName: 'application/msword', icon: '<i class="fa-solid fa-file-word"></i>' },
    { typeName: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', icon: '<i class="fa-solid fa-file-word"></i>' },
    { typeName: 'application/pdf', icon: '<i class="fa-solid fa-file-pdf"></i>' },
    { typeName: 'image/jpg', icon: '<i class="fa-solid fa-file-image"></i>' },
    { typeName: 'image/jpeg', icon: '<i class="fa-solid fa-file-image"></i>' },
    { typeName: 'image/png', icon: '<i class="fa-solid fa-file-image"></i>' },
    { typeName: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', icon: '<i class="fa-solid fa-file-excel"></i>' },
    { typeName: 'text/plain', icon: '<i class="fa-solid fa-file-lines"></i>' },
]

function setFileData(target) {
    const progressArea = document.querySelector(".progress-area"),
        uploadedArea = document.querySelector(".uploaded-area");
        let file = target.files[0]
    //console.log(file.type)
    const fileType = file.type
    let fileImg = '';
    objIconsFiles.forEach((elem) => {
        if (elem.typeName == fileType) {
            fileImg = elem.icon
        }

    })
    progressArea.innerHTML = "";
    let uploadedHTML = `<li class="row">
                            <div class="content upload">
                              ${fileImg}
                              <div class="details">
                                <span class="name">${file.name} • Добавлено</span>
                                <span class="size">${file.size}</span>
                              </div>
                            </div>
                            <i class="fa-solid fa-xmark delete_upload_file" onclick="deleteUploadFile(this)"></i>
                          </li>`;

    uploadedArea.insertAdjacentHTML("afterbegin", uploadedHTML);

    uploadFile(file)
}

function deleteUploadFile(elem) {
    console.log(elem.parentNode)
    elem.parentNode.remove()
}
function uploadFile(files) {
    const maxFileSize = 1048576;//10мб
    const currentUrl = '';
   
}