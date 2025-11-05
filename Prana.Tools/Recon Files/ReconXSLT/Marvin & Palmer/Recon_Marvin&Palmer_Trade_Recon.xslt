<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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


	<xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
			  <xsl:with-param name="Number" select="normalize-space(COL126)" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and COL9= 'TRD'">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="COL69" />
			  </xsl:variable>
			  <xsl:variable name="varCUSIP">
				  <xsl:value-of select="normalize-space(COL75)" />
			  </xsl:variable>
			  <xsl:variable name="varISIN">
				  <xsl:value-of select="normalize-space(COL74)" />
			  </xsl:variable>


			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol" />
			  </xsl:variable>

			  <xsl:variable name="varAsset">
				  <xsl:choose>
					  <xsl:when test="contains(COL45,'Equities')">
						  <xsl:value-of select="'Equity'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			    <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>
               <!-- <xsl:when test="$varCUSIP = ''"> -->
                  <!-- <xsl:value-of select="''" /> -->
                <!-- </xsl:when> -->
                 <xsl:when test="string-length($varCUSIP)=7">
                  <xsl:value-of select="''" />
                </xsl:when>
				  
                <xsl:otherwise>
                 <xsl:value-of select="normalize-space(substring-before(COL71,'US'))" />
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>
            <SEDOL>
              <xsl:choose>               
                <xsl:when test="string-length($varCUSIP)= 7">
                  <xsl:value-of select="$varCUSIP" />
                </xsl:when>      
								
                <xsl:otherwise>
				<xsl:value-of select="normalize-space(substring-before(COL71,'US'))" />
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>
			  
			  
			  <xsl:variable name="PB_FUND_NAME" select="COL15" />
			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
			  </xsl:variable>
			  <AccountName>
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select="$PRANA_FUND_NAME" />
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$PB_FUND_NAME" />
					  </xsl:otherwise>
				  </xsl:choose>
			  </AccountName>


			  <Quantity>
              <xsl:choose>
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
				  <xsl:with-param name="Number" select="COL128"/>
              </xsl:call-template>
            </xsl:variable>

            <AvgPX>
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
            </AvgPX>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL138"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>
                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

			  <xsl:variable name="varfee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="normalize-space(COL144)"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <SecFee>
				  <xsl:choose>
					  <xsl:when test="$varfee &gt; 0">
						  <xsl:value-of select="$varfee"/>
					  </xsl:when>
					  <xsl:when test="$varfee &lt; 0">
						  <xsl:value-of select="$varfee * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </SecFee>

			  <xsl:variable name="varOtherBrokerFees">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="normalize-space(COL149)"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <OtherBrokerFees>
				  <xsl:choose>
					  <xsl:when test="$varOtherBrokerFees &gt; 0">
						  <xsl:value-of select="$varOtherBrokerFees"/>
					  </xsl:when>
					  <xsl:when test="$varOtherBrokerFees &lt; 0">
						  <xsl:value-of select="$varOtherBrokerFees * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </OtherBrokerFees>
			  
			  <xsl:variable name="PB_BROKER_NAME">
				  <xsl:value-of select="normalize-space(COL18)"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_BROKER_ID">
				  <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PB_BROKER_NAME]/@PranaBroker"/>
			  </xsl:variable>

			  <CounterPartyID>
				  <xsl:choose>
					  <xsl:when test="number($PRANA_BROKER_ID!='')">
						  <xsl:value-of select="$PRANA_BROKER_ID"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CounterPartyID>

            <xsl:variable name="Currency" select="COL88"/>
            <CurrencySymbol>
              <xsl:value-of select="$Currency"/>
            </CurrencySymbol>

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL132"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$NetNotionalValue &gt; 0">
                  <xsl:value-of select="$NetNotionalValue"/>
                </xsl:when>
                <xsl:when test="$NetNotionalValue &lt; 0">
                  <xsl:value-of select="$NetNotionalValue * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>


            <xsl:variable name="varNetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>

			 
            <TradeDate>
              <xsl:value-of select="COL114"/>
            </TradeDate>

            <SettlementDate>
              <xsl:value-of select ="COL115"/>
            </SettlementDate>

           <xsl:variable name ="Side" >
              <xsl:value-of select="COL10"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when  test="$Side='SEL'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
                <xsl:when  test="$Side='BUY'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
			  
			  <PBAssetType>
				  <xsl:value-of select="COL45"/>
			  </PBAssetType>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


