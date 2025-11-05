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
  <xsl:template name="CheckCurrency">
    <xsl:param name="myCurrency"/>
    <xsl:param name="myFXRate"/>
    <xsl:param name="myNumber"/>
    <xsl:choose>
      <xsl:when test="$myCurrency = 'USD'">
        <xsl:choose>
          <xsl:when test="$myNumber &gt; 0">
            <xsl:value-of select="$myNumber div $myFXRate"/>
          </xsl:when>
          <xsl:when test="$myNumber &lt; 0">
            <xsl:value-of select="($myNumber * (-1)) div $myFXRate"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="0"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$myNumber &gt; 0">
            <xsl:value-of select="$myNumber "/>
          </xsl:when>
          <xsl:when test="$myNumber &lt; 0">
            <xsl:value-of select="($myNumber * (-1)) "/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="0"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL8)"/>
          </xsl:call-template>
        </xsl:variable>
       
         <xsl:variable name="varSCurrency">
              <xsl:value-of select="COL15"/>
            </xsl:variable>
           
        <xsl:if test="number($varQuantity)" >
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="varSymbol">
              <xsl:value-of select="(normalize-space(COL5))" />
            </xsl:variable>
            <xsl:variable name="varSEDOL">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
             <xsl:variable name="varFXRate">
              <xsl:value-of select="COL10"/>
            </xsl:variable>
             <FXRate>
              <xsl:choose>
                <xsl:when test="$varFXRate &gt; 0">
                  <xsl:value-of select="$varFXRate" />
                </xsl:when>
                <xsl:when test="$varFXRate &lt; 0">
                  <xsl:value-of select="$varFXRate * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

           <SettlCurrencyName>
              <xsl:value-of select="$varSCurrency"/>
            </SettlCurrencyName>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>

                <xsl:when test="$varSEDOL!='*'">
                  <xsl:value-of select="''" />
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME" />
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSEDOL!='*'">
                  <xsl:value-of select="$varSEDOL" />
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>
            <xsl:variable name="varTransactionType" select="COL4"/>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varTransactionType='Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varTransactionType='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varTransactionType='Sell Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$varTransactionType='Buy to close'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>
            <xsl:variable name="PB_FUND_NAME" select="''"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>
            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
            <xsl:variable name="varFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              
              <xsl:call-template name="CheckCurrency">
                <xsl:with-param name="myCurrency" select="$varSCurrency"/>
                <xsl:with-param name="myFXRate" select="$varFXRate"/>
                <xsl:with-param name="myNumber" select="$varFees"/>
              </xsl:call-template>
            
            </Fees>
           

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>
           
            <CostBasis>

              <xsl:call-template name="CheckCurrency">
                <xsl:with-param name="myCurrency" select="$varSCurrency"/>
                <xsl:with-param name="myFXRate" select="$varFXRate"/>
                <xsl:with-param name="myNumber" select="$varCostBasis"/>
              </xsl:call-template>
              
            </CostBasis>
            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>

              <xsl:call-template name="CheckCurrency">
                <xsl:with-param name="myCurrency" select="$varSCurrency"/>
                <xsl:with-param name="myFXRate" select="$varFXRate"/>
                <xsl:with-param name="myNumber" select="$varCommission"/>
              </xsl:call-template>
            
            </Commission>
           
           

            <xsl:variable name="varDate" select="COL2"/>
            <PositionStartDate>
              <xsl:value-of select="$varDate"/>
            </PositionStartDate>
            <xsl:variable name="varSDate" select="COL3"/>
            <PositionSettlementDate>
              <xsl:value-of select="$varSDate"/>
            </PositionSettlementDate>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
