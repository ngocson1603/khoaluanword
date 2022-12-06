// async function myDisplay() {
//     let myPromise = new Promise(function (resolve, reject) {
//         resolve("I love You !!");
//     });
//     document.getElementById("demo").innerHTML = await myPromise;
// }

// myDisplay();

// "use strict"
var fullname = "nguyen ngoc son"
function test() {
    age = 18;
}
test()
console.log(fullname)
console.log(age)

//this
// function Name(ten, ho) {
//     this.ten = ten
//     this.ho = ho
//     this.ss = function () {
//         console.log(this)
//     }
// }
// const son = new Name(`son`, `nguyen`)
// console.log(son.ss())

//undified
// function test() {
//     console.log(this)
// }
// test()

//this trong ham tao
// function Name(ten, ho) {
//     this.ten = ten
//     this.ho = ho
// }
// const son = new Name(`son`, `nguyen`)
// console.log(son.ten) this = son

//bind
// this.name = `son`
// this.ho = `nguyen`
// const son1 = {
//     name: "son",
//     ho: "nguyen",
//     getFull() {
//         return `${this.name} ${this.ho}`
//     }
// }
// const son2 = {
//     name: "son1",
//     ho: "nguyen1",
// }
// console.log(son1.getFull())
// const get = son1.getFull.bind(son2)
// console.log(get())

//bind truyen doi so
// this.name = `son`
// this.ho = `nguyen`
// const son1 = {
//     name: "son",
//     ho: "nguyen",
//     getFull(data1, data2) {
//         console.log(data1, data2)
//         return `${this.name} ${this.ho}`
//     }
// }
// const son2 = {
//     name: "son1",
//     ho: "nguyen1",
// }
// //console.log(son1.getFull())
// const get = son1.getFull.bind(son2, 'test1', 'test2')
// console.log(get('tes', 'tes1'))

//call
// const son1 = {
//     name: "son",
//     ho: "nguyen",
//     getFull() {
//         console.log(`${this.name} ${this.ho}`)
//     }
// }
// const son2 = {
//     name: "son1",
//     ho: "nguyen1",
// }
// son1.getFull.call(son2)

//this call
// this.name = `son`
// this.ho = `nguyen`
// function getFull() {
//     console.log(`${this.name} ${this.ho}`)
// }
// getFull.call(this)

//oop
// function Animal(name, weight) {
//     this.name = name
//     this.weight = weight
// }
// function Chicken(name, weight, legs) {
//     Animal.call(this, name, weight)
//     this.legs = legs
// }
// const son = new Chicken("son nguyen", 2, 2)
// console.log(son)

//muon ham
// function logger() {
//     const arr = [...arguments]
//     console.log(arr)
// }

// logger(1, 2, 3, 4, 5)

//apply
// const son1 = {
//     name: "son",
//     ho: "nguyen",
// }
// function getFull(create, mess) {
//     return `${create}${this.name}${this.ho} ${mess}`
// }
// let result = getFull.apply(son1, ['xin chao', 'chao ban'])
// console.log(result)
// result = getFull.call(son1, 'xin chao', 'chao ban')
// console.log(result)

//muon ham
// const teacher = {
//     firstName: "sonG",
//     lastName: "NguyenG",
//     isOnline: false,
//     goOnline() {
//         this.isOnline = true
//         console.log(`${this.firstName} ${this.lastName} is Online`)
//     },
//     goOffline() {
//         this.isOffline = false
//         console.log(`${this.firstName} ${this.lastName} is Offline`)
//     }
// }
// const me = {
//     firstName: "Son",
//     lastName: "Nguyen",
//     isOnline: false,
// }
// console.log('teacher: ', teacher.isOnline)
// teacher.goOnline()
// console.log('teacher: ', teacher.isOnline)
// console.log('------------------------------------------')
// console.log('student: ', me.isOnline)
// teacher.goOnline.apply(me)
// console.log('student: ', me.isOnline)

//oop
// function Animal(name, weight) {
//     this.name = name
//     this.weight = weight
// }
// function Chicken(name, weight, legs) {
//     Animal.apply(this, [name, weight])
//     this.legs = legs
// }
// const son = new Chicken("son nguyen", 2, 2)
// console.log(son)

//closure
// function createStorage(key) {
//     const store = JSON.parse(localStorage.getItem(key)) ?? {};
//     const save = () => {
//         localStorage.setItem(key, JSON.stringify(store));
//     };

//     const storage = {
//         get(key) {
//             return store[key];
//         },

//         set(key, value) {
//             store[key] = value;
//             save();
//         },

//         remove(key) {
//             delete store[key];
//             save();
//         },
//     };
//     return storage;
// }

// const setting = createStorage("settings");
// console.log(setting.set("name"));
// setting.set("name", "nguyen ngoc son");