/**
 * Adobe Edge: symbol definitions
 */
(function($, Edge, compId){
//images folder
var im='images/';

var fonts = {};
   fonts['\'Helvetica Neue\'']='';


var resources = [
];
var symbols = {
"stage": {
   version: "2.0.1",
   minimumCompatibleVersion: "2.0.0",
   build: "2.0.1.268",
   baseState: "Base State",
   initialState: "Base State",
   gpuAccelerate: false,
   resizeInstances: false,
   content: {
         dom: [
         {
            id:'back',
            type:'image',
            rect:['0px','0px','640px','1136px','auto','auto'],
            fill:["rgba(0,0,0,0)",im+"back.png",'0px','0px']
         },
         {
            id:'Text',
            type:'text',
            rect:['177px','597px','auto','auto','auto','auto'],
            opacity:0.72549438476562,
            text:"Processing order...",
            font:['\'Helvetica Neue\'',35,"rgba(255,255,255,1.00)","300","none",""]
         },
         {
            id:'gear',
            type:'image',
            rect:['272px','445px','96px','101px','auto','auto'],
            fill:["rgba(0,0,0,0)",im+"gear.png",'0px','0px']
         },
         {
            id:'success2x',
            type:'image',
            rect:['250px','223px','140px','140px','auto','auto'],
            fill:["rgba(0,0,0,0)",im+"success%402x.png",'0px','0px']
         },
         {
            id:'text',
            display:'none',
            type:'image',
            rect:['87px','414px','466px','265px','auto','auto'],
            fill:["rgba(0,0,0,0)",im+"text.png",'0px','0px']
         },
         {
            id:'ui',
            display:'none',
            type:'image',
            rect:['69px','856px','501px','215px','auto','auto'],
            fill:["rgba(0,0,0,0)",im+"ui.png",'0px','0px']
         }],
         symbolInstances: [

         ]
      },
   states: {
      "Base State": {
         "${_back}": [
            ["style", "left", '0px'],
            ["style", "top", '0px']
         ],
         "${_Text}": [
            ["style", "top", '597px'],
            ["style", "font-family", '\'Helvetica Neue\''],
            ["style", "opacity", '0.72549438476562'],
            ["color", "color", 'rgba(255,255,255,1.00)'],
            ["style", "font-weight", '300'],
            ["style", "left", '177px'],
            ["style", "font-size", '35px']
         ],
         "${_gear}": [
            ["style", "top", '465px'],
            ["style", "opacity", '1'],
            ["style", "left", '272px'],
            ["transform", "rotateZ", '0deg']
         ],
         "${_Stage}": [
            ["color", "background-color", 'rgba(255,255,255,1)'],
            ["style", "width", '640px'],
            ["style", "height", '1136px'],
            ["style", "overflow", 'hidden']
         ],
         "${_text}": [
            ["style", "top", '453px'],
            ["style", "opacity", '0'],
            ["style", "left", '87px'],
            ["style", "display", 'none']
         ],
         "${_ui}": [
            ["style", "top", '856px'],
            ["style", "opacity", '0'],
            ["style", "left", '69px'],
            ["style", "display", 'none']
         ],
         "${_success2x}": [
            ["style", "top", '263px'],
            ["style", "opacity", '0'],
            ["style", "left", '250px'],
            ["style", "display", 'block']
         ]
      }
   },
   timelines: {
      "Default Timeline": {
         fromState: "Base State",
         toState: "",
         duration: 2302,
         autoPlay: true,
         timeline: [
            { id: "eid20", tween: [ "style", "${_success2x}", "opacity", '1', { fromValue: '0'}], position: 1750, duration: 329 },
            { id: "eid13", tween: [ "style", "${_success2x}", "display", 'none', { fromValue: 'block'}], position: 0, duration: 0 },
            { id: "eid12", tween: [ "style", "${_success2x}", "display", 'block', { fromValue: 'none'}], position: 1750, duration: 0 },
            { id: "eid45", tween: [ "style", "${_ui}", "opacity", '1', { fromValue: '0'}], position: 1884, duration: 418, easing: "easeOutCubic" },
            { id: "eid36", tween: [ "style", "${_ui}", "display", 'none', { fromValue: 'none'}], position: 0, duration: 0, easing: "easeOutCubic" },
            { id: "eid37", tween: [ "style", "${_ui}", "display", 'block', { fromValue: 'none'}], position: 1884, duration: 0, easing: "easeOutCubic" },
            { id: "eid31", tween: [ "style", "${_text}", "opacity", '1', { fromValue: '0'}], position: 1885, duration: 417, easing: "easeOutCubic" },
            { id: "eid35", tween: [ "style", "${_text}", "top", '414px', { fromValue: '453px'}], position: 1884, duration: 418, easing: "easeOutCubic" },
            { id: "eid3", tween: [ "transform", "${_gear}", "rotateZ", '180deg', { fromValue: '0deg'}], position: 0, duration: 1750 },
            { id: "eid27", tween: [ "style", "${_text}", "display", 'none', { fromValue: 'none'}], position: 0, duration: 0, easing: "easeOutCubic" },
            { id: "eid26", tween: [ "style", "${_text}", "display", 'block', { fromValue: 'none'}], position: 1885, duration: 0, easing: "easeOutCubic" },
            { id: "eid8", tween: [ "style", "${_Text}", "opacity", '0', { fromValue: '0.725494'}], position: 1542, duration: 208 },
            { id: "eid25", tween: [ "style", "${_success2x}", "top", '223px', { fromValue: '263px'}], position: 1750, duration: 329, easing: "easeOutCubic" },
            { id: "eid11", tween: [ "style", "${_gear}", "opacity", '0', { fromValue: '1'}], position: 1542, duration: 208 }         ]
      }
   }
}
};


Edge.registerCompositionDefn(compId, symbols, fonts, resources);

/**
 * Adobe Edge DOM Ready Event Handler
 */
$(window).ready(function() {
     Edge.launchComposition(compId);
});
})(jQuery, AdobeEdge, "EDGE-30914175");
