
//Position class
function Paint(id, lineWidth, strokeStyle){
    this.canvas = document.querySelector(id);
    this.context = this.canvas.getContext('2d');
    this.lineWidth = lineWidth;
    this.strokeStyle = strokeStyle;
    
    this.isDrawing = false;
    this.startPos = {x: 0, y: 0};

    this.prevRect = null;
    this.totalRects = [];
    this.currentRects = [];
}

Paint.prototype.setLineWidth = function(lineWidth){
    this.lineWidth = lineWidth;
}

Paint.prototype.setStrokeStyle = function(strokeStyle){
    this.strokeStyle = strokeStyle;
}

Paint.prototype.getStartPos = function(){
    return this.startPos;
}

Paint.prototype.setStartPos = function(pos){
    this.startPos.x = pos.x;
    this.startPos.y = pos.y;
    return this;
}

Paint.prototype.setIsDrawing = function(value){
    this.isDrawing = value;
    return this;
}

Paint.prototype.getIsDrawing = function(){
    return this.isDrawing;
}

Paint.prototype.setPrevRect = function(startX, startY, width, height, endX, endY){
    this.prevRect = {startX, startY, width, height, endX, endY};
    return this;
}

Paint.prototype.resetPrevRect = function(){
    this.prevRect = null;
    return this;
}

Paint.prototype.saveRect = function (){
    const index = this.currentRects.length;

    //if undo few times and draw new one,
    // you have to drop the datas from the current index.
    if(this.totalRects.length > index){
        while(this.totalRects.length > index ){
            this.totalRects.pop();
        }
    }
    //and set new one.
    if(this.prevRect){
        this.currentRects[index] = this.prevRect;
        this.totalRects[index] = this.prevRect;
    }

    return this;
}

Paint.prototype.getCurrentRects = function (){
    return this.currentRects;
}

Paint.prototype.resetTotalRects = function (){
    this.totalRects = [];
    this.currentRects = [];
    return this;
}

Paint.prototype.drawPrevRects = function (){
    const ctx = this.context;
    ctx.lineWidth = this.lineWidth;
    ctx.strokeStyle = this.strokeStyle;
    ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

    //previous rects
    const currentRects = this.getCurrentRects();
    
    currentRects.forEach( list => {
        ctx.strokeRect(list.startX, list.startY, list.width, list.height);    
    })

}


Paint.prototype.drawRects = function (x, y){
    const ctx = this.context;
    const startPos = this.getStartPos();
    const width = x - startPos.x;
    const height = y - startPos.y;

    this.drawPrevRects();

    //update current rect
    ctx.strokeRect(startPos.x, startPos.y, width, height);
    this.setPrevRect(startPos.x, startPos.y, width, height, x, y);

}

Paint.prototype.setCanvasSize = function (width, height){
    this.canvas.style.width = `${width}px`;
    this.canvas.style.height = `${height}px`;
    this.canvas.width = width;
    this.canvas.height = height;
}

Paint.prototype.resetDrawing = function (){
    const ctx = this.context;
    ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    return this;
}


Paint.prototype.undoCurrentRects = function (){
    this.currentRects.pop();

    if(this.currentRects.length === 0 ){
        this.resetPrevRect();
    }

    return this;
}

Paint.prototype.redoCurrentRects = function (){
    const currentLength = this.currentRects.length;
    this.currentRects.push(
        this.totalRects[currentLength]
    );

    return this;
}

Paint.prototype.undoDrawing = function (){
    const currentRects = this.getCurrentRects();
    if(currentRects.length > 0){
        this.undoCurrentRects();

        this.drawPrevRects();
    }
    return this;
}


Paint.prototype.redoDrawing = function (){
    if(this.currentRects.length < this.totalRects.length){
        this.redoCurrentRects();
        
        this.drawPrevRects();
    }

    return this;
}

Paint.prototype.resetCanvasSize = function (){
    const width = this.canvas.parentElement.getBoundingClientRect().width;
    const height = this.canvas.parentElement.getBoundingClientRect().height;
    this.canvas.width = width;
    this.canvas.height = height;
    this.canvas.style.width = `${width}px`;
    this.canvas.style.height = `${height}px`;
}

let lineWidth = 10;
const paint = new Paint('#rectCanvas', lineWidth, '#ff0000');

const uploadBox = document.querySelector('#uploadBox');
const resetBox = document.querySelector('#resetBox');
const resetImage = document.querySelector('#resetImage');
const resetDrawing = document.querySelector('#resetDrawing');
const uploadedImage = document.querySelector('#uploadedImage');
const uploadImage = document.querySelector('#uploadImage');
const redoDrawing = document.querySelector('#redoDrawing');
const undoDrawing = document.querySelector('#undoDrawing');
const strokeColor = document.querySelector('#strokeColor');
const lineWidthThin = document.querySelector('#lineWidth_thin');
const lineWidthThick = document.querySelector('#lineWidth_thick');
const send = document.querySelector('#send');

strokeColor.value = "#ff0000";
//initialize default value
uploadImage && (uploadImage.value = "");
uploadedImage.src = "";
uploadedImage.alt = "";

//upload event
uploadImage && uploadImage.addEventListener('change', e => {
    const files = e.target.files;
    if(files.length > 0){
        let src = URL.createObjectURL(files[0]);
        document.body.style.cursor = "wait";        

        let img = new Image();
        img.src = src;
        img.onload = function (){
            document.body.style.cursor = "default";
            let width = this.width > 1024 ? 1024 : this.width;
            let height = this.height > 900 ? 900 : this.height;
            paint.setCanvasSize(width, height);
            uploadedImage.width = width;
            uploadedImage.height = height;
            uploadedImage.src = src;
            uploadedImage.alt = src;

            uploadedImage.classList.remove('d-none');
            uploadBox && uploadBox.classList.add('d-none');
            resetBox && resetBox.classList.remove('d-none');
            paint.canvas.style.cursor = "crosshair";
        }
    }
})

//reset upload event
resetImage && resetImage.addEventListener('click', e => {
    e.preventDefault();
    uploadedImage.src = "";
    uploadedImage.alt = "";
    uploadedImage.width = "";
    uploadedImage.height = "";
    uploadImage && (uploadImage.value = "");

    uploadedImage.classList.add('d-none');
    uploadBox && uploadBox.classList.remove('d-none');
    resetBox && resetBox.classList.add('d-none');

    paint.resetTotalRects();
    paint.resetCanvasSize();
    paint.canvas.style.cursor = "default";

    uploadImage && uploadImage.click();
})

resetDrawing.addEventListener('click', e => {
    e.preventDefault();
    paint.resetDrawing();
    paint.resetTotalRects();
})

redoDrawing.addEventListener('click', e => {
    e.preventDefault();
    paint.redoDrawing();
})

undoDrawing.addEventListener('click', e => {
    e.preventDefault();
    paint.undoDrawing();
})

strokeColor.addEventListener('change', e => {
    const color = e.target.value;
    paint.setStrokeStyle(color);
    paint.drawPrevRects();
})

lineWidthThin.addEventListener('click', e => {
    e.preventDefault();
    lineWidth = lineWidth - 1;
    if(lineWidth <= 5){
        lineWidth = 2;
    }
    paint.setLineWidth(lineWidth);
    paint.drawPrevRects();
})

lineWidthThick.addEventListener('click', e => {
    e.preventDefault();
    lineWidth = lineWidth + 1;
    if(lineWidth >= 18){
        lineWidth = 18;
    }
    paint.setLineWidth(lineWidth);
    paint.drawPrevRects();
})

send.addEventListener('click', e => {
    e.preventDefault();
    const rects = paint.getCurrentRects();
    console.log(rects);
})

//canvas mouse click event to start drawing
paint.canvas.addEventListener('mousedown', e => {
    if(!uploadedImage.classList.contains('d-none')){
        const x = e.offsetX;
        const y = e.offsetY;
        const startPos = { x, y };
        paint.setStartPos(startPos);
        paint.setIsDrawing(true);
    }
})


paint.canvas.addEventListener('mouseup', e => {
    const isDrawing = paint.getIsDrawing();
    if(isDrawing){
        paint.setIsDrawing(false);
        paint.saveRect();
    }
})


paint.canvas.addEventListener('mousemove', e => {
    const isDrawing = paint.getIsDrawing();
    if(isDrawing){
        const x = e.offsetX;
        const y = e.offsetY;
        paint.drawRects(x, y);
    }
})



