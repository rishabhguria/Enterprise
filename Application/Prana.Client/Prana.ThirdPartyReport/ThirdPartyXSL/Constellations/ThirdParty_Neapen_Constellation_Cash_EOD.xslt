<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <!--<xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>-->
  
  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>


  <!--<xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="'&#160;'"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>-->

  <!--<xsl:variable name = "BlankCount_Root" >
    <xsl:call-template name="noofBlanks">
      <xsl:with-param name="count1" select="(6) - string-length($varOptionUnderlying)" />
    </xsl:call-template>
  </xsl:variable>-->


  <!--<xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>-->
  
  <!--<xsl:variable name="varZeros">
    <xsl:call-template name="noofzeros">
      <xsl:with-param name="count" select="(10-string-length(PBUniqueID)- string-length($varFundID))"/>
    </xsl:call-template>
  </xsl:variable>-->

  <!--<xsl:variable name="varUniqueID">
    <xsl:value-of select="concat($varZeros, PBUniqueID, $varFundID)"/>
  </xsl:variable>-->


  <xsl:template match="/NewDataSet">
    
    

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          
          <RowHeader>
            <xsl:value-of select="'false'"/>
          </RowHeader>


          <FileHeader>
            <xsl:value-of select="'true'"/>
          </FileHeader>

          <FileFooter>
            <xsl:value-of select="'true'"/>
          </FileFooter>

          <!--<xsl:variable name="varaDate">
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </xsl:variable>-->
        
         

         <xsl:variable name="PB_NAME" select="'IB'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
          
          
           <xsl:variable name="varaDate">
            <xsl:value-of select="TradeDate"/>
          </xsl:variable>
          
           <xsl:variable name="varAccountName">
            <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
          </xsl:variable>
          
         <xsl:variable name="varCashUpdateType">
          <xsl:value-of select="'  1'"/>
        </xsl:variable>
         
          <xsl:variable name = "varAddBlanks_CashAdjustetedAmt" >
          <xsl:call-template name="noofBlanks">
            <xsl:with-param name="count1" select="21" />
          </xsl:call-template>
        </xsl:variable>
          
        <xsl:variable name = "varAddBlanks_CurrentDateCashAmt_Integer" >
         <xsl:value-of select="substring-before(CashValueLocal,'.')"/>
        </xsl:variable>
          
          
            <xsl:variable name = "varAddBlanks_CurrentDateCashAmt_Decimal" >
             <xsl:value-of select="substring-after(CashValueLocal,'.')"/>
            </xsl:variable>
          
     <xsl:variable name = "varAddBlanks_CurrentDateCashAmt" >
            <xsl:call-template name="noofBlanks">
      <xsl:with-param name="count1" select="(21 - string-length(CashValueLocal))" />
    </xsl:call-template>
  </xsl:variable>
          
          <xsl:variable name = "varAddBlanks_CTC" >
              <xsl:call-template name="noofBlanks">
               <xsl:with-param name="count1" select="14" />
          </xsl:call-template>
        </xsl:variable>

          <xsl:variable name = "varAddBlanks_Security" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="40" />
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_Notes" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="40" />
            </xsl:call-template>
          </xsl:variable>


          <!-- <xsl:variable name = "varPrimeBroker" > -->
            
           <!-- <xsl:value-of select="'MSPB '"/> -->
          <!-- </xsl:variable> -->

          <xsl:variable name = "varPrimeBroker" > 
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="5" />
            </xsl:call-template>
           </xsl:variable>

          <xsl:variable name = "varAddBlanks_Strategy" >         
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="40" />
            </xsl:call-template>
          </xsl:variable>


          <xsl:variable name = "varAddBlanks_FundClass" >            
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="15" />
            </xsl:call-template>
          </xsl:variable>



          <xsl:variable name = "varAddBlanks_NumberOfUnits" >          
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="21" />
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name = "varAddBlanks_UnitValue" >
            <xsl:call-template name="noofBlanks">
              <xsl:with-param name="count1" select="21" />
            </xsl:call-template>
          </xsl:variable>


          <CashEOD>
            <xsl:value-of select="concat($varaDate,$varAccountName,$varCashUpdateType,$varAddBlanks_CashAdjustetedAmt,$varAddBlanks_CurrentDateCashAmt,CashValueLocal,
           $varAddBlanks_CurrentDateCashAmt,CashValueLocal,LocalCurrency,$varAddBlanks_CTC,$varAddBlanks_Security,$varAddBlanks_Notes,$varPrimeBroker,$varAddBlanks_Strategy,
                          $varAddBlanks_FundClass,$varAddBlanks_NumberOfUnits,$varAddBlanks_UnitValue)"/>
          </CashEOD>
          
          
   <!--<xsl:variable name = "BlankCount_Root" >
    <xsl:call-template name="noofBlanks">
      <xsl:with-param name="count1" select="(6) - string-length($varOptionUnderlying)" />
    </xsl:call-template>
  </xsl:variable>-->
          
    <!--<xsl:variable name="varBlanks">
    <xsl:call-template name="noofBlanks">
      <xsl:with-param name="count1" select="(8)-string-length($varaDate)"/>
    </xsl:call-template>
  </xsl:variable>

   <xsl:variable name="varAccountName">
    <xsl:value-of select="concat($varBlanks,'UAT_US')"/>
  </xsl:variable>
          <AccountName>
            <xsl:value-of select="$varAccountName"/>
          </AccountName>
          
          
 <xsl:variable name="varCashBlanks">
    <xsl:call-template name="noofBlanks">
      <xsl:with-param name="count1" select="(8)-string-length($varAccountName)"/>
    </xsl:call-template>
  </xsl:variable>
          
   <xsl:variable name="varCashUpdateType">
    <xsl:value-of select="concat($varCashBlanks,'3')"/>
  </xsl:variable>
          
          <CashUpdateType>
            <xsl:value-of select="$varCashUpdateType"/>
          </CashUpdateType>

          <CashAdjAmt>
            <xsl:value-of select="$varCashBlanks"/>
          </CashAdjAmt>

          <OTDCA>
            <xsl:value-of select="CashValueLocal"/>
          </OTDCA>

          <OSDCA>
            <xsl:value-of select="CashValueLocal"/>
          </OSDCA>

          <CurrencyCode>
            <xsl:value-of select="LocalCurrency"/>
          </CurrencyCode>

          <CTC>
            <xsl:value-of select="''"/>
          </CTC>

          <asofdate>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </asofdate>


          <PrimeBroker>
            <xsl:value-of select="'MSPB'"/>
          </PrimeBroker>

          <Strategy>
            <xsl:value-of select="'EMTY'"/>
          </Strategy>

          <FundClass>
            <xsl:value-of select="''"/>
          </FundClass>

          <NumberOfUnits>
            <xsl:value-of select="''"/>
          </NumberOfUnits>

          <UnitValue>
            <xsl:value-of select="''"/>
          </UnitValue>-->

      
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>