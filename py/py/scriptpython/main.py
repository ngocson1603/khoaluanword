import cvxpy as cp
import numpy

# Problem data.
m = 30
n = 20
numpy.random.seed(1)
A = numpy.random.randn(m, n)
b = numpy.random.randn(m)

# Construct the problem.
x = cp.Variable(n)
objective = cp.Minimize(cp.sum_squares(A @ x - b))
constraints = [0 <= x, x <= 1]
prob = cp.Problem(objective, constraints)

# The optimal objective is returned by prob.solve().
result = prob.solve()
# The optimal value for x is stored in x.value.
print(x.value)
# The optimal Lagrange multiplier for a constraint
# is stored in constraint.dual_value.
print(constraints[0].dual_value)


# def sdp_function(x, y):
#     # Define optimization variables
#     X = cp.Semidefinite(3)

#     # Define constraints
#     constraints = [X >> 0, X[0, 0] == x, X[1, 1] == y]

#     # Define objective function
#     obj = cp.Minimize(cp.trace(X))

#     # Form and solve optimization problem
#     prob = cp.Problem(obj, constraints)
#     prob.solve()

#     # Return result
#     return X.value


# sdp_function(3, 3)
