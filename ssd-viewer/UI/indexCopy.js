
//Position class
//Paint 라는 함수를 선언
function Paint(id, lineWidth, strokeStyle){ //Paint라는 함수 안에 id, lineWidth, strokeStyle 이라는 매개변수 정의
    //this 키워드 -> 자기 자신이 가진 속성을 접근하고 싶을 때 표시할때는 this 라는 키워드 정의 ()=> 처럼 화살표 함수에서 쓰이는 this와는 차이점을 갖는다
    // this 키워드는 객체 내부의 메소드에서 객체 자신을 나타내는 키워드
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

// canvas line(점선) setting
Paint.prototype.setLineWidth = function(lineWidth){//Paint 라는 객체 자료형(객체를 기반으로 하는 자료형, 객체의 틀을 의미) 이름.prototype(이곳에 속성과 메소드를 추가하면 해당 객체 전체에서 사용할 수 있음).메소드이름 = function(){} 여기선 setLineWidth 이라는 속성을 추가
    this.lineWidth = lineWidth;
}

// canvas stroke(선) setting 
Paint.prototype.setStrokeStyle = function(strokeStyle){
    this.strokeStyle = strokeStyle;
}

// canvas 시작위치 가져오기
Paint.prototype.getStartPos = function(){
    return this.startPos; // Paint 함수에서 정한 startpos 값
}

// canvas 시작위치 setting
Paint.prototype.setStartPos = function(pos){
    this.startPos.x = pos.x; // start 위치 x값
    this.startPos.y = pos.y; // start 위치 y값
    return this;  // 위 값 return
}

// canvas 드로잉(그릴때) 값 setting
Paint.prototype.setIsDrawing = function(value){
    this.isDrawing = value; 
    return this;
}

// canvas 드로잉(그릴때) 값 가져오기
Paint.prototype.getIsDrawing = function(){
    return this.isDrawing;
}

// canvas 이전 도형 setting
Paint.prototype.setPrevRect = function(startX, startY, width, height, endX, endY){
    this.prevRect = {startX, startY, width, height, endX, endY}; 
    return this;
}

// canvas 도형 초기화
Paint.prototype.resetPrevRect = function(){
    this.prevRect = null; // 초기화이니 null
    return this;
}

// canvas 도형 저장
Paint.prototype.saveRect = function (){
    const index = this.currentRects.length;

    //if undo few times and draw new one,
    // you have to drop the datas from the current index. 몇 번 실행 취소하고 새로 그리면 현재 인덱스에서 데이터를 삭제.
    if(this.totalRects.length > index){
        while(this.totalRects.length > index ){
            this.totalRects.pop();
        }
    }
    //and set new one. 새로운 하나를 생성
    if(this.prevRect){
        this.currentRects[index] = this.prevRect;
        this.totalRects[index] = this.prevRect;
    }

    return this;
}

// canvas 현재 도형
Paint.prototype.getCurrentRects = function (){
    return this.currentRects;
}

// canvas 도형 전체 초기화
Paint.prototype.resetTotalRects = function (){
    this.totalRects = [];
    this.currentRects = [];
    return this;
}

// canvas 이전 도형 스타일 그리기
Paint.prototype.drawPrevRects = function (){
    const ctx = this.context;
    ctx.lineWidth = this.lineWidth;
    ctx.strokeStyle = this.strokeStyle;
    ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

    //previous rects 이전 도형들
    const currentRects = this.getCurrentRects();
    
    // 이전 도형들에 대한 각각의 값 적용
    currentRects.forEach( list => {
        ctx.strokeRect(list.startX, list.startY, list.width, list.height);    
    })

}

// canvas 도형 그리기
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

// canvas 사이즈 setting
Paint.prototype.setCanvasSize = function (width, height){
    this.canvas.style.width = `${width}px`;
    this.canvas.style.height = `${height}px`;
    this.canvas.width = width;
    this.canvas.height = height;
}

// canvas 그리기 초기화
Paint.prototype.resetDrawing = function (){
    const ctx = this.context;
    ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    return this;
}

// canvas 현재 도형 실행취소
Paint.prototype.undoCurrentRects = function (){
    this.currentRects.pop(); //pop() 메서드는 배열에서 마지막 요소를 제거하고 그 요소를 반환.

    if(this.currentRects.length === 0 ){
        this.resetPrevRect();
    }

    return this;
}

// cavas 현재 도형 다시 실행
Paint.prototype.redoCurrentRects = function (){
    const currentLength = this.currentRects.length;
    this.currentRects.push(
        this.totalRects[currentLength]
    );

    return this;
}

// canvas 그리기 실행 취소 
Paint.prototype.undoDrawing = function (){
    const currentRects = this.getCurrentRects();
    if(currentRects.length > 0){
        this.undoCurrentRects();

        this.drawPrevRects();
    }
    return this;
}

// canvas 그리기 재실행
Paint.prototype.redoDrawing = function (){
    if(this.currentRects.length < this.totalRects.length){
        this.redoCurrentRects();
        
        this.drawPrevRects();
    }

    return this;
}

// canvas 사이즈 초기화
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



