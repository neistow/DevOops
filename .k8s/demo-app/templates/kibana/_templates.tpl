{{- define "kibana.labels" -}}
{{ include "app.labels" . }}
app.kubernetes.io/component: kibana
{{- end }}

{{- define "kibana.selectorLabels" -}}
{{ include "app.selectorLabels" . }}
app.kubernetes.io/component: kibana
{{- end }}
