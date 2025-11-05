<?xml version="1.0" encoding="utf-8"?>

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

 

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL10,'CALL') or contains(COL10,'PUT')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL10),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL10),'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL10),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL10),'/'),'/'),' ')"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(COL10,' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(normalize-space(COL10),' '),' '),' '),' '),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
			
		</xsl:if>
	</xsl:template>



	<xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:if test="number(COL11)">
          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name="varPBName">
              <xsl:value-of select="'NB'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSIBlanks">
              <xsl:call-template name="noofBlanks">
                <xsl:with-param name="count1" select="(6-string-length(substring-before(COL4,' ')))"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="concat(substring-before(COL4,' '),$varOSIBlanks,substring-after(COL4,' '))"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="COL26"/>
            </xsl:variable>

            <xsl:variable name="varExYear">
              <xsl:value-of select="substring-after(substring-after($varOptionExpiry,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varStrike">
              <xsl:value-of select="COL27"/>
            </xsl:variable>

            <xsl:variable name="varPutCall">
              <xsl:value-of select="COL25"/>
            </xsl:variable>


            <xsl:variable name="varUnderlying">
              <xsl:value-of select="substring-before(COL12,' ')"/>
            </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="'Equity'"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varQuantity">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varMarketValueLocal">
              <xsl:value-of select="COL15"/>
            </xsl:variable>


            <xsl:variable name="varMarketValueBase">
              <xsl:value-of select="COL15"/>
            </xsl:variable>
            
            <!--<xsl:variable name="varSMRequest">
              <xsl:value-of select="'TRUE'"/>
            </xsl:variable>-->
            
            <FundName>
              <xsl:value-of select="$PRANA_FUND_NAME"/>
            </FundName>

			  <CompanyName>
				  <xsl:value-of select ="COL10"/>
			  </CompanyName>


			  <xsl:variable name="AssetType">
				  <xsl:choose>

					  <xsl:when test="contains(COL10,'CALL') or contains(COL10,'PUT') ">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="Equity"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  
			  <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_Symbol_NAME != ''">
                  <xsl:value-of select="$PRANA_Symbol_NAME"/>
                </xsl:when>


				  <xsl:when test="$AssetType='EquityOption'">
					  <xsl:call-template name="Option">
						  <xsl:with-param name="Symbol" select="COL10"/>
						  <xsl:with-param name="Suffix" select="''"/>
					  </xsl:call-template>
				  </xsl:when>
				  
				  <xsl:when test="COL8!=''">
					  <xsl:value-of select="COL8"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="$PB_Symbol_NAME"/>
				  </xsl:otherwise>
               
              </xsl:choose>
            </Symbol>

          
            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>


            <!--QUANTITY-->

            <Quantity>
              <xsl:value-of select="$varQuantity"/>
            </Quantity>

			  <Side>
				  <xsl:choose>
					  <xsl:when test="$AssetType='EquityOption'">
						  <xsl:choose>
							  <xsl:when test="$varQuantity &gt; 0">
								  <xsl:value-of select="'Buy to Open'"/>
							  </xsl:when>

							  <xsl:when test="$varQuantity &lt; 0">
								  <xsl:value-of select="'Sell to Open'"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$varQuantity &gt; 0">
								  <xsl:value-of select="'Buy'"/>
							  </xsl:when>

							  <xsl:when test="$varQuantity &lt; 0">
								  <xsl:value-of select="'Sell short'"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>

						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>
				  
				  
			  </Side>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarkPrice))">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>


            <MarketValue>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarketValueLocal))">
                  <xsl:value-of select="$varMarketValueLocal"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarketValueBase))">
                  <xsl:value-of select="$varMarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>

			  <SMRequest>
				  <xsl:value-of select="'true'"/>
			  </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
