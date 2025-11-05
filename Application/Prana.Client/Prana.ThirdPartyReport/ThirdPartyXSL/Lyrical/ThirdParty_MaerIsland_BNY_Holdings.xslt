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

        <AccountID>
          <xsl:value-of select="'AccountID'"/>
        </AccountID>

        <Holding>
          <xsl:value-of select="'Holding'"/>
        </Holding>

        <SecurityType>
          <xsl:value-of select="'Security Type'"/>
        </SecurityType>

        <SEDOL>
          <xsl:value-of select="'SEDOL'"/>
        </SEDOL>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <Security>
          <xsl:value-of select="'Security'"/>
        </Security>

        <SecurityDescription>
          <xsl:value-of select="'Security Description'"/>
        </SecurityDescription>

        <Shares>
          <xsl:value-of select="'Shares'"/>
        </Shares>

        <MarketValue>
          <xsl:value-of select="'Market Value'"/>
        </MarketValue>

       
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
              <xsl:when test="TaxLotState='Amended'">
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

          <xsl:variable name="varSideMult">
            <xsl:choose>
              <xsl:when test="substring(Side,1,1) = 'B'">
                <xsl:value-of select="1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="-1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <AccountID>
            <xsl:value-of select="AccountName"/>
          </AccountID>

          <Holding>
            <xsl:choose>
              <xsl:when test="substring(Side,1,1) = 'B'">
                <xsl:value-of select="'Long'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'Short'"/>
              </xsl:otherwise>
            </xsl:choose>
          </Holding>
          
          <SecurityType>
            <xsl:value-of select="Country"/>
          </SecurityType>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <Security>
            <xsl:value-of select="Symbol"/>
          </Security>

          <SecurityDescription>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDescription>

          <Shares>
            <xsl:value-of select="AllocatedQty*$varSideMult"/>
          </Shares>

          <MarketValue>
            <xsl:value-of select="SecFees*AllocatedQty*Multiplier*$varSideMult"/>
          </MarketValue>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
