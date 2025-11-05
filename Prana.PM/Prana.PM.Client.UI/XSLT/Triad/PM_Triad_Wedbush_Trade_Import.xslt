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

  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
    <xsl:param name="varPutCall"/>
    <xsl:if test="$varPutCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07' ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$varPutCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
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
    <xsl:if test="contains(normalize-space(COL18),'P ') or contains(normalize-space(COL18),'C ')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="normalize-space(COL36)"/>
      </xsl:variable>
	  <xsl:variable name="Date" select="substring(substring-after(normalize-space(COL5),' '),1,6)"/>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring($Date,5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring($Date,3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring($Date,1,2)"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="COL35"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(COL38,'#.00')"/>
      </xsl:variable>
      <xsl:variable name="MonthCode">
        <xsl:call-template name="MonthCodevar">
          <xsl:with-param name="Month" select="$ExpiryMonth"/>
          <xsl:with-param name="varPutCall" select="$PutORCall"/>
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) and COL3 !='T' and COL3 !='C'">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Fidelity'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL18"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="Asset" select="''"/>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="number(COL38)">
                  <xsl:call-template name="Option">
                  <xsl:with-param name="Symbol"/>
                  </xsl:call-template>
                </xsl:when>
                <xsl:when test ="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="COL2"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>

                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

			  <xsl:variable name="OrfFees">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL10"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <OrfFee>
				  <xsl:choose>
					  <xsl:when test="$OrfFees &gt; 0">
						  <xsl:value-of select="$OrfFees"/>

					  </xsl:when>
					  <xsl:when test="$OrfFees &lt; 0">
						  <xsl:value-of select="$OrfFees * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </OrfFee>

			  <xsl:variable name="OtherBrokerFees">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL22"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <Fees>
				  <xsl:choose>
					  <xsl:when test="$OtherBrokerFees &gt; 0">
						  <xsl:value-of select="$OtherBrokerFees"/>

					  </xsl:when>
					  <xsl:when test="$OtherBrokerFees &lt; 0">
						  <xsl:value-of select="$OtherBrokerFees * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </Fees>


			  <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
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

            <xsl:variable name ="Side" select="COL3"/>
            <SideTagValue>
				<xsl:choose>
					<xsl:when test="COL3 = 'P' and COL34 = '2' and not(number(COL38))">
						<xsl:value-of select="'1'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'P' and COL34 = '3'  and not(number(COL38))">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'S' and COL34 = '2'  and not(number(COL38))">
						<xsl:value-of select="'2'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'S'and COL34 = '3'   and not(number(COL38))">
						<xsl:value-of select="'5'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'SS' and COL34 = '2' and not(number(COL38))">
						<xsl:value-of select="'2'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'SS'and COL34 = '3'  and not(number(COL38))">
						<xsl:value-of select="'5'"/>
					</xsl:when>
					<!--For option trades-->
					<xsl:when test="COL3 = 'P' and COL34 = '2' and (number(COL38)) ">
						<xsl:value-of select="'A'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'P' and COL34 = '3' and (number(COL38)) ">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'S' and COL34 = '2' and (number(COL38)) ">
						<xsl:value-of select="'D'"/>
					</xsl:when>
					<xsl:when test="COL3 = 'S' and COL34 = '3' and (number(COL38)) ">
						<xsl:value-of select="'C'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
            </SideTagValue>


            <xsl:variable name="SecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>

            <SecFee>
              <xsl:choose>
                <xsl:when test="$SecFee &gt; 0">
                  <xsl:value-of select="$SecFee"/>

                </xsl:when>
                <xsl:when test="$SecFee &lt; 0">
                  <xsl:value-of select="$SecFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </SecFee>


			  <xsl:variable name="PB_BROKER_NAME">
				  <xsl:value-of select="COL29"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_BROKER_ID">
				  <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
			  </xsl:variable>

			  <CounterPartyID>
				  <xsl:choose>
					  <xsl:when test="number($PRANA_BROKER_ID)">
						  <xsl:value-of select="$PRANA_BROKER_ID"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CounterPartyID>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <TradeDate>
              <xsl:value-of select="normalize-space(COL26)"/>
            </TradeDate>

			  <PositionSettlementDate>
				  <xsl:value-of select="normalize-space(COL27)"/>
			  </PositionSettlementDate>

            <PositionStartDate>
              <xsl:value-of select="normalize-space(COL26)"/>
            </PositionStartDate>
            
          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>