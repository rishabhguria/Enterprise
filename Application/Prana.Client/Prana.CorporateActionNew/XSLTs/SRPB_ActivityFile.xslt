<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:ms="urn:schemas-microsoft-com:xslt"
  version="1.0">

<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL7 != 'Symbol' and  COL7 != '' and COL19 != 0">
          
					<PositionMaster>
						<OrigSymbol>
							<xsl:value-of select="COL7"/>
						</OrigSymbol>
						<ExDivDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL20"/>
              </xsl:call-template>
						</ExDivDate>
						<RecordDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL21"/>
              </xsl:call-template>
						</RecordDate>
						<DivPayoutDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL22"/>
              </xsl:call-template>
            </DivPayoutDate>
						<DivRate>
							<xsl:value-of select="COL19"/>
						</DivRate>
          </PositionMaster>
          
        </xsl:if >
			</xsl:for-each>	      
		</DocumentElement>
	</xsl:template>

  
  
  
  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->
    
    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>
    
    <xsl:value-of select="$nMonth"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDay"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>


</xsl:stylesheet>
