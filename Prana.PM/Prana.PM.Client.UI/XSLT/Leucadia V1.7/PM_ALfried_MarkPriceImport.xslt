<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>


		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
  <xsl:template name="MonthNo">
    <xsl:param name="varMonth"/>

    <xsl:choose>
      <xsl:when test ="$varMonth= 'A' or $varMonth= 'M'">
        <xsl:value-of select ="01"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'B' or $varMonth= 'N'">
        <xsl:value-of select ="02"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'C' or $varMonth= 'O'">
        <xsl:value-of select ="03"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'D' or $varMonth= 'P'">
        <xsl:value-of select ="04"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'E' or $varMonth= 'Q'">
        <xsl:value-of select ="05"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'F' or $varMonth= 'R'">
        <xsl:value-of select ="6"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'G' or $varMonth= 'S'">
        <xsl:value-of select ="7"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'H' or $varMonth= 'T'">
        <xsl:value-of select ="08"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'I' or $varMonth= 'U'">
        <xsl:value-of select ="09"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'J' or $varMonth= 'V'">
        <xsl:value-of select ="10"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'K' or $varMonth= 'W'">
        <xsl:value-of select ="11"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'L' or $varMonth= 'X'">
        <xsl:value-of select ="12"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>

    <xsl:variable name="varRoot">	
      <xsl:value-of select="substring-before($varBBCode,'1')"/>
    </xsl:variable>

    <xsl:variable name="varUnderlyingLength">
      <xsl:value-of select="string-length($varRoot)"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +6)), '#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +3),2)"/>
    </xsl:variable>

    <xsl:variable name="varExpiryDay">
      <xsl:choose>
        <xsl:when test="substring($varExDay,1,1)= '0'">
          <xsl:value-of select="substring($varExDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varExDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring(COL3,($varUnderlyingLength + 5),1)"/>
    </xsl:variable>

    <xsl:variable name="varExMonth">
      <xsl:call-template name="MonthNo">
        <xsl:with-param name="varMonth" select="$varMonthCode"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name='varThirdFriday'>
		<xsl:choose>
			<xsl:when test='number($varExYear) and number($varExMonth)'>
				<xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
			</xsl:when>
		</xsl:choose>   
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
        <xsl:value-of select="normalize-space(concat('O:',$varRoot, ' ', $varExYear,$varMonthCode,$varStrike))"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="normalize-space(concat('O:',$varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test ="number(COL13)">
			<!--<xsl:if test ="number(COL5) and COL1='31405346' and not(COL1='31405346' and (contains(COL6,'CALL ')!=false or contains(COL6,'PUT ')!=false))" >-->
          <PositionMaster>

          


            
           


			  <!--<xsl:variable name = "PB_COMPANY" >
				  <xsl:value-of select="COL3"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='WedbushM']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			  </xsl:variable>-->

			  <xsl:variable name="PB_NAME">
				  <xsl:value-of select="'NT'"/>
			  </xsl:variable>

			 	<xsl:variable name = "PB_SYMBOL_NAME" >
				  <xsl:value-of select ="COL8"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>

				  <xsl:when test ="COL3 !=''">
					  <xsl:value-of select ="COL3"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="$PB_SYMBOL_NAME"/>
				  </xsl:otherwise>
               
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
			  
			  <xsl:variable name="AvgPrice">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL26 div COL13"/>
				  </xsl:call-template>
			  </xsl:variable>
            <MarkPrice>
				<xsl:choose>
					<xsl:when test="$AvgPrice &gt; 0">
						<xsl:value-of select="$AvgPrice"/>

					</xsl:when>
					<xsl:when test="$AvgPrice &lt; 0">
						<xsl:value-of select="$AvgPrice * (-1)"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>

				</xsl:choose>
            </MarkPrice>

            <Date>
              <xsl:value-of select="''"/>
            </Date>


          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
