{{- define "redis.labels" -}}
{{ include "app.labels" . }}
app.kubernetes.io/component: redis
{{- end }}

{{- define "redis.selectorLabels" -}}
{{ include "app.selectorLabels" . }}
app.kubernetes.io/component: redis
{{- end }}

