const form = document.getElementById("form_upload"),
    fileInput = document.querySelector(".file-input"),
    progressArea = document.querySelector(".progress-area"),
    uploadedArea = document.querySelector(".uploaded-area");
console.log(form)
//form.addEventListener("click", () => {
//    fileInput.click();
//});

function start(target) {
    let file = target.files[0];
    if (file) {
        let fileName = file.name;
        if (fileName.length >= 12) {
            let splitName = fileName.split('.');
            fileName = splitName[0].substring(0, 13) + "... ." + splitName[1];
        }
        uploadFile(fileName.type);
    }
}

function uploadFile(name) {
    const formUploadElem = document.getElementById('form_upload_file')
    const maxFileSize = 1048576;//10мб
    const currentUrl = '';


    //const typeDoc = 'application/msword';
    //const typeDocx = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
    //const typePdf = 'application/pdf';
    //const typePng = 'image/png';
    //const typeJpeg = 'image/jpeg';
    //const typeJpg = 'image/jpg';
    //const typeExcel = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
    //const typeTxt = 'text/plain';
    let Data = new FormData();
    //$(files).each(function (index, file) {
    //    if ((file.size <= maxFileSize) && ((file.type == typeDoc) || (file.type == typeDocx) || (file.type == typePdf) || (file.type == typeJpeg) || (file.type == typeJpg) || (file.type == typeTxt) || (file.type == typePng) || (file.type == typeExcel))) {
    //        Data.append('files[]', file);
    //        dataOfFiles.fileName = file.name;
    //        dataOfFiles.fileType = file.type;
    //        fileToBase64(file);
    //        let fileInputElement = document.getElementById('file-input');
    //        // fileInputElement.classList.add('kek');
    //        // console.log(fileInputElement);
    //        alert('Файл был успешно загружен!');
    //    } else {
    //        alert('Файлы данного типа не поддерживаются');
    //    }
    //});
    
    //let xhr = new XMLHttpRequest();
    //xhr.open("POST", "php/upload.php");
    //xhr.upload.addEventListener("progress", ({ loaded, total }) => {
    //    let fileLoaded = Math.floor((loaded / total) * 100);
    //    let fileTotal = Math.floor(total / 1000);
    //    let fileSize;
    //    (fileTotal < 1024) ? fileSize = fileTotal + " KB" : fileSize = (loaded / (1024 * 1024)).toFixed(2) + " MB";
    //    let progressHTML = `<li class="row">
    //                      <i class="fas fa-file-alt"></i>
    //                      <div class="content">
    //                        <div class="details">
    //                          <span class="name">${name} • Uploading</span>
    //                          <span class="percent">${fileLoaded}%</span>
    //                        </div>
    //                        <div class="progress-bar">
    //                          <div class="progress" style="width: ${fileLoaded}%"></div>
    //                        </div>
    //                      </div>
    //                    </li>`;
    //    uploadedArea.classList.add("onprogress");
    //    progressArea.innerHTML = progressHTML;
    //    if (loaded == total) {
    //        progressArea.innerHTML = "";
    //        let uploadedHTML = `<li class="row">
    //                        <div class="content upload">
    //                          <i class="fas fa-file-alt"></i>
    //                          <div class="details">
    //                            <span class="name">${name} • Uploaded</span>
    //                            <span class="size">${fileSize}</span>
    //                          </div>
    //                        </div>
    //                        <i class="fas fa-check"></i>
    //                      </li>`;
    //        uploadedArea.classList.remove("onprogress");
    //        uploadedArea.insertAdjacentHTML("afterbegin", uploadedHTML);
    //    }
    //});
    //let data = new FormData(formUploadElem);
    //xhr.send(data);
}