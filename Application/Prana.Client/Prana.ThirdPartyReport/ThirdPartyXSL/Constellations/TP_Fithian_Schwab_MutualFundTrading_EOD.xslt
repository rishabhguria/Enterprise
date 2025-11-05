<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail[UDAAssetName='Money Market' and CounterParty='CHAS']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <FileHeader>
            <xsl:value-of select="'true'"/>
          </FileHeader>

          <xsl:variable name = "varRecodType" >
            <xsl:value-of select="'TR'"/>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_CustomerAccount" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="(8 - string-length(AccountNo))" />
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name = "varEmpty" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="1" />
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select="'BUY  DLRS'"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select="'SELL SHRS'"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_Side" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="(9 - string-length($varSide))" />
            </xsl:call-template>
          </xsl:variable>


          <xsl:variable name="varQuantityBuy">
            <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select="''"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_Quantity" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="(16 - string-length($varQuantityBuy))" />
            </xsl:call-template>
          </xsl:variable>

       
          <xsl:variable name="varQuantitySell">
            <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">                
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_QuantitySell" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="(16 - string-length($varQuantitySell))" />
            </xsl:call-template>
          </xsl:variable>
          
          <xsl:variable name = "varEmpty1" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="1" />
            </xsl:call-template>
          </xsl:variable>


          <xsl:variable name = "varQuantityLinked" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="5" />
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name = "varSymbol" >
            <xsl:value-of select="Symbol"/>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_Symbol" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="(5 - string-length($varSymbol))" />
            </xsl:call-template>
          </xsl:variable>
          
          <xsl:variable name = "varTransactionFeeIndicator" >
            <xsl:value-of select="'N'"/>
          </xsl:variable>

          
        <xsl:variable name = "varDividendGainsInstruction">        
            <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select="'CC'"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select="'  '"/>
              </xsl:when>
            </xsl:choose>       
        </xsl:variable>

          <xsl:variable name = "varNewMoney" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="5" />
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_AccountNo" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="(8 - string-length(AccountNo))" />
            </xsl:call-template>
          </xsl:variable>
		  
		  <xsl:variable name = "varAddBlanks_PBUniqueID" >
            <xsl:choose>
			<xsl:when test="string-length(PBUniqueID) &lt; 7">
			    <xsl:value-of select="concat(00,PBUniqueID)"/>
			</xsl:when>
			<xsl:when test="string-length(PBUniqueID) &lt; 8">
			    <xsl:value-of select="concat(0,PBUniqueID)"/>
			</xsl:when>
			<xsl:otherwise>
			<xsl:value-of select="PBUniqueID"/>
			</xsl:otherwise>
			
			</xsl:choose>
          </xsl:variable>
		  
		  <!-- <xsl:variable name = "varAddBlanks_PBUniqueID" > -->
            <!-- <xsl:call-template name="noofBlanks"> -->
              <!-- <xsl:with-param name="count1" select="(8 - string-length(PBUniqueID))" /> -->
            <!-- </xsl:call-template> -->
          <!-- </xsl:variable> -->

          <TradesDetail>
            <xsl:value-of select="concat($varRecodType,AccountNo,$varAddBlanks_AccountNo,$varAddBlanks_CustomerAccount,$varEmpty,$varSide,$varAddBlanks_Side,$varQuantityBuy,$varAddBlanks_Quantity,$varQuantitySell
			,$varAddBlanks_QuantitySell,$varEmpty1,$varQuantityLinked,$varSymbol,$varAddBlanks_Symbol,$varEmpty1,$varTransactionFeeIndicator,$varEmpty1,$varDividendGainsInstruction,$varEmpty1,
                          $varAddBlanks_PBUniqueID,$varEmpty1,0,$varNewMoney)"/>
          </TradesDetail>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
