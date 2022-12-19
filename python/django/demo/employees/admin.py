from django.contrib import admin
from .models import employees
# Register your models here.


class EmployeeAdmin(admin.ModelAdmin):
    list_display = ('employee_id', 'department_id', 'name', 'age')
    search_fields = ['name']
    list_filter = ('employee_id', 'department_id', 'name', 'age')


admin.site.register(employees, EmployeeAdmin)
