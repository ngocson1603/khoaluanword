from django.shortcuts import render, redirect
from .models import employees as employee_model
from home.models import department as department_model
# Create your views here.


def get_employees(request, id):
    employee_list = employee_model.objects.filter(department_id=id)
    department = department_model.objects.get(department_id=id)
    return render(request, 'employees.html', {'employee_list': employee_list, 'department': department})


def get_employee_form(request):
    department_list = department_model.objects.filter()
    return render(request, 'employeeForm.html', {'department_list': department_list})


def add_employee(request):
    if request.method == 'POST':
        department_id = request.POST['department']
        name = request.POST['fullname']
        age = request.POST['age']
        avatar = request.FILES['avatar']
        cv = request.FILES['cv']

        department = department_model.objects.get(department_id=department_id)
        employee = employee_model.objects.create(department_id=department,
                                                 name=name,
                                                 age=age,
                                                 avatar=avatar,
                                                 cv=cv)
        employee.save()
        return redirect('/department/' + str(department_id))
    else:
        return render(request, 'error.html')
