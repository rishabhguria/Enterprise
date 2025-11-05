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

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth=01">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=02">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=03">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=04">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=05">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=06">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=07">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=08">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=09">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=10">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=13">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=14">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=15">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=16">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=17">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=18">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=19">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=20">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=21">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=22">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=23">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=24">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster[substring(COL1,40,1) = 'O' or substring(COL1,40,1) = 'C' or substring(COL1,40,1) = 'A' or substring(COL1,40,1) = 'G']">

				<PositionMaster>

					<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(substring(COL1,61,30))"/>
				  <xsl:variable name="PRANA_SYMBOL_NAME">
					  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Newland_GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
				  </xsl:variable>

				  <xsl:variable name ="varSymbol">
					  <xsl:value-of select ="normalize-space(substring(COL1,26,8))"/>
				  </xsl:variable>


				  <xsl:variable name="varUnderlying" select="normalize-space(substring(COL1,201,8))"/>
				  <xsl:variable name="varOptExpiration_Year" select="substring(COL1,48,2)" />
				  <xsl:variable name="varOptExpiration_Month" select="substring(COL1,147,2)" />
          <xsl:variable name="Date" select="substring(COL1,44,2)" />
				  <xsl:variable name="Strike_PriceInt" select="number(substring(COL1,149,8))"/>
				  <xsl:variable name="Strike_Price" select="concat($Strike_PriceInt,'.',substring(COL1,157,8))"/>

				  <xsl:variable name ="varCallPutCode">
					  <xsl:choose>
						  <xsl:when test ="substring(COL1,147,2) &lt; 13 and substring(COL1,147,2) &gt; 0">
							  <xsl:value-of select ="'1'"/>
						  </xsl:when>
						  <xsl:when test ="substring(COL1,147,2) &gt; 12">
							  <xsl:value-of select ="'0'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

				  <xsl:variable name = "varMonthCode" >
					  <xsl:call-template name="MonthCode">
						  <xsl:with-param name="varMonth" select="$varOptExpiration_Month" />
					  </xsl:call-template>
				  </xsl:variable>


				  <xsl:variable name="varStrike">
					  <xsl:choose>
						  <xsl:when test="$varCallPutCode !=''">
							  <xsl:variable name ="varStrikeDecimal" select ="substring-after($Strike_Price,'.')"/>
							  <xsl:variable name ="varStrikeInt" select ="substring-before($Strike_Price,'.')"/>
							  <xsl:choose>
								  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
									  <xsl:value-of select ="concat($Strike_Price,'0')"/>
								  </xsl:when>
								  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
									  <xsl:value-of select ="$Strike_Price"/>
								  </xsl:when>
								  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
									  <xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select ="concat($Strike_Price,'.00')"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME != ''">
						  <Symbol>
							  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
						  </Symbol>
					  </xsl:when>
					  <xsl:when test ="$varCallPutCode !=''">
              
              <!--<xsl:variable name="varThirdFriday">
                <xsl:value-of select =" my:Now(number(concat('20',$varOptExpiration_Year)),number(substring(COL1,42,2)))"/>
              </xsl:variable>

              <xsl:variable name="varIsFlex">
                <xsl:choose>
                  <xsl:when test="(substring-before(substring-after($varThirdFriday,'/'),'/') + 1) = normalize-space($Date)">
                    <xsl:value-of select="0"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>-->

              <xsl:variable name="varDate">
                <xsl:choose>
                  <xsl:when test="substring($Date,1,1)='0'">
                    <xsl:value-of select="substring($Date,2,1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$Date"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              
						  <Symbol>
                <!--<xsl:choose>
                  <xsl:when test="$varIsFlex = 0">
                    <xsl:value-of select="concat('O:',$varUnderlying,' ',$varOptExpiration_Year,$varMonthCode,$varStrike)"/>
                  </xsl:when>
                  <xsl:otherwise>-->
                    <xsl:value-of select="concat('O:',$varUnderlying,' ',$varOptExpiration_Year,$varMonthCode,$varStrike, 'D',$varDate)"/>
                  <!--</xsl:otherwise>
                </xsl:choose>-->
						  </Symbol>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test ="$PB_COMPANY_NAME = 'UNITED STATES TREASURY NOTE'">
								  <Symbol>
									  <xsl:value-of select="substring(COL1,14,9)" />
								  </Symbol>

							  </xsl:when>
							  <xsl:otherwise>
								  <Symbol>
									  <xsl:value-of select="$varSymbol"/>
								  </Symbol>

							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose >
				  <PBSymbol>
					  <xsl:value-of select="concat('varSymbol-',$varSymbol,'  ,varCallPutCode--',$varCallPutCode,',PRANA_SYMBOL_NAME--',$PB_COMPANY_NAME)"/>
				  </PBSymbol>


				  <xsl:variable name ="varPrice">
					  <xsl:value-of select ="substring(COL1,235,16)"/>
				  </xsl:variable>

				  <xsl:variable name ="varPriceInt">
					  <xsl:value-of select ="substring($varPrice,1,8)"/>
				  </xsl:variable>

				  <xsl:variable name ="varPriceFrac">
					  <xsl:value-of select ="substring($varPrice,9,8)"/>
				  </xsl:variable>

				  <xsl:variable name ="varFormatPrice">
					  <xsl:value-of select="concat($varPriceInt,'.',$varPriceFrac)"/>
				  </xsl:variable>


				  <MarkPrice>
				  <xsl:choose>
					  <xsl:when test="boolean(number($varFormatPrice))">
						  <xsl:value-of select="format-number($varFormatPrice, '####.0000000')"/>
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
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
