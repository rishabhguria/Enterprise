<?xml version="1.0" encoding="UTF-8"?>
<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <!--<InstrumentSubType>
					<xsl:value-of select="'Instrument Sub Type'"/>
				</InstrumentSubType>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<LifeCycle>
					<xsl:value-of select="'Life Cycle'"/>
				</LifeCycle>-->

        <AssetType>
          <xsl:value-of select="'Asset Type'"/>
        </AssetType>

        <Contract>
          <xsl:value-of select="'Contract'"/>
        </Contract>

        <BBGKey>
          <xsl:value-of select="'BBG Key'"/>
        </BBGKey>

        <B_S>
          <xsl:value-of select="'B/S'"/>
        </B_S>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <Open_Close>
          <xsl:value-of select="'Open/Close'"/>
        </Open_Close>

        <LongDesc>
          <xsl:value-of select="'Long Desc'"/>
        </LongDesc>

        <Cusip>
          <xsl:value-of select="'Cusip'"/>
        </Cusip>

        <Expiry>
          <xsl:value-of select="'Expiry'"/>
        </Expiry>


        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="true"/>
          </RowHeader>

          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="true"/>
          </IsCaptionChangeRequired>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>


          <!--<InstrumentSubType>
						<xsl:value-of select="Asset"/>
					</InstrumentSubType>

					<Comments>
						<xsl:value-of select="''"/>
					</Comments>-->

          <!--Exercise / Assign Need To Ask-->
          <xsl:variable name="varLifeCycle">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amemded'">
                <xsl:value-of select="'Replace'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'Delete'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Sent'">
                <xsl:value-of select="'Expire'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="TaxLotState"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <!--<LifeCycle>
						<xsl:value-of select="$varLifeCycle"/>
					</LifeCycle>-->

          <!--Exercise / Assign Need To Ask-->

          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated' or TaxLotState='Sent'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amemded'">
                <xsl:value-of select="'R'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'D'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <AssetType>
            <xsl:value-of select="translate(Asset, $varSmall, $varCapital)"/>
          </AssetType>

          <Contract>
            <xsl:value-of select="BBCode"/>
          </Contract>

			<xsl:variable name = "PB_Root_Name" >
				<xsl:value-of select="normalize-space(substring(BBCode,1,2))"/>
			</xsl:variable>
			
			<xsl:variable name = "PB_Asset" >
				<xsl:value-of select="normalize-space(Asset)"/>
			</xsl:variable>

			<xsl:variable name="PRANA_BBKey_NAME">
				<xsl:value-of select="document('../ReconMappingXml/Bloombergsuffix.xml')/Bloombergsuffix/PB[@Name='BNY']/SymbolData[@Asset=$PB_Asset and @Underlying=$PB_Root_Name]/@Suffix"/>
			</xsl:variable>

          <BBGKey>
			  <xsl:choose>
				  <xsl:when test ="$PRANA_BBKey_NAME != ''">
					  <xsl:value-of select ="$PRANA_BBKey_NAME"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select ="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </BBGKey>

          <B_S>
            <xsl:value-of select="substring(Side,1,1)"/>
          </B_S>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <Open_Close>
            <xsl:value-of select="''"/>
          </Open_Close>

          <LongDesc>
            <xsl:value-of select="FullSecurityName"/>
          </LongDesc>

          <Cusip>
            <xsl:value-of select="CUSIP"/>
          </Cusip>

          <Expiry>
			  <xsl:choose>
				  <xsl:when test ="AssetID = 1 or AssetID = 5">
					  <xsl:value-of select ="''"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="ExpirationDate"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </Expiry>
          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  
</xsl:stylesheet>
